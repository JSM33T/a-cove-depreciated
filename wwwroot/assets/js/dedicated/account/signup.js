var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { acInit, acToast, acPostData, acFormHandler } from '../../global.js';
const firstName = document.getElementById('firstName');
const lastName = document.getElementById('lastName');
const userName = document.getElementById('userName');
const emailId = document.getElementById('emailId');
const pWord = document.getElementById('pWord');
const pWordConfirm = document.getElementById('pWordConfirm');
const signupBtn = document.getElementById('submitBtn');
const otpModal = new bootstrap.Modal(document.getElementById('otpMdl'));
const otpSubmit = document.getElementById("submitOtp");
const otpVal = document.getElementById("otpVal");
acInit([
    () => acFormHandler('signup-form', submitLoginForm),
    () => acFormHandler('otp-form', verifyDeets)
]);
const submitLoginForm = function () {
    return __awaiter(this, void 0, void 0, function* () {
        const signupdata = {
            firstName: firstName.value,
            lastName: lastName.value,
            userName: userName.value,
            eMail: emailId.value,
            password: pWord.value,
            confirmPassword: pWordConfirm.value
        };
        if (firstName.value.length <= 1) {
            acToast("error", "Username too short");
        }
        else if (userName.value.length < 4) {
            acToast("error", "Username too short");
        }
        else if (pWord.value.length <= 6) {
            acToast('error', 'Password should be atleast 6 characters long');
        }
        else if (pWordConfirm.value != pWord.value) {
            acToast('error', 'Passwords dont match');
        }
        else {
            postToSignUpApi(signupdata);
        }
    });
};
function postToSignUpApi(signupdata) {
    return __awaiter(this, void 0, void 0, function* () {
        const apiUrl = '/api/account/signup';
        signupBtn.innerHTML = "Wait...";
        signupBtn.classList.add('pe-none');
        try {
            const response = yield acPostData(apiUrl, signupdata);
            acToast(response.type, response.data);
            console.log(response);
            if (response.type === "ok") {
                signupBtn.innerHTML = "Logging in...";
                otpModal.show();
            }
        }
        catch (error) {
            console.error('Error during login:', error);
        }
        finally {
            signupBtn.innerHTML = "Log In";
            signupBtn.classList.remove('pe-none');
        }
    });
}
;
const verifyDeets = function () {
    return __awaiter(this, void 0, void 0, function* () {
        otpSubmit.innerHTML = "Wait...";
        otpSubmit.classList.add('pe-none');
        console.log(otpVal.value);
        console.log(userName.value);
        try {
            const dt = {
                OTP: otpVal.value.trim(),
                UserName: userName.value.trim()
            };
            const response = yield acPostData("/api/user/verification", dt);
            console.log(dt);
            console.log(response);
            if (response.type === "ok") {
                otpModal.hide();
                acToast("success", "user verified redirecting to login page...");
                setTimeout(redirect, 2000);
            }
            else {
                console.log("error", response.data);
            }
        }
        catch (error) {
            console.error('Error during login:', error);
            acToast('error', 'something went wrong');
        }
        finally {
            otpSubmit.innerHTML = "Verify";
            otpSubmit.classList.remove('pe-none');
        }
    });
};
const redirect = () => window.location.href = "/";

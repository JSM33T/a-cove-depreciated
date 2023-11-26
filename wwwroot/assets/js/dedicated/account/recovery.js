var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { acToast, acPostData, acFormHandler, acInit } from '../../global.js';
const userNameInput = document.getElementById('userName');
const submitBtn = document.getElementById('recSubmitBtn');
const otpModal = new bootstrap.Modal(document.getElementById('otpMdl'));
const otpSubmit = document.getElementById("submitOtp");
const otpVal = document.getElementById("otpVal");
acInit([
    () => acFormHandler('recovery-form', submitUserName),
    () => acFormHandler('otp-form', recoverAccount)
]);
function submitUserName() {
    return __awaiter(this, void 0, void 0, function* () {
        if (userNameInput.value.trim().length <= 1) {
            acToast("error", "Username too short");
        }
        else {
            yield postToLoginApi(userNameInput.value);
        }
    });
}
function postToLoginApi(usernameval) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const response = yield acPostData('/api/account/recover', {
                userName: usernameval
            });
            console.log(response);
            acToast(response.type, response.data);
            if (response.type === "ok") {
                submitBtn.innerHTML = "Proceed";
                const lastLink = localStorage.getItem("curr_link");
                otpModal.show();
            }
        }
        catch (error) {
            console.error('Error during login:', error);
            acToast('error', 'Something went wrong');
        }
        finally {
            submitBtn.innerHTML = "Log In";
        }
    });
}
const recoverAccount = function () {
    return __awaiter(this, void 0, void 0, function* () {
        otpSubmit.innerHTML = "Wait...";
        otpSubmit.classList.add('pe-none');
        console.log(otpVal.value);
        try {
            const dt = {
                OTP: otpVal.value.trim(),
            };
            const response = yield acPostData("/api/account/loginviaotp", dt);
            console.log(response);
            if (response.type === "ok") {
                otpModal.hide();
                acToast("success", "Logging into your acc. Make sure to set your password");
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

import { User } from '../../Interfaces/user.interface.js';
import { acInit, acToast, acPostData, acFormHandler } from '../../global.js';
declare const bootstrap: { Modal: new (arg0: HTMLElement | null) => any; };
const firstName = document.getElementById('firstName') as HTMLInputElement;
const lastName = document.getElementById('lastName') as HTMLInputElement;
const userName = document.getElementById('userName') as HTMLInputElement;
const emailId = document.getElementById('emailId') as HTMLInputElement;
const pWord = document.getElementById('pWord') as HTMLInputElement;
const pWordConfirm = document.getElementById('pWordConfirm') as HTMLInputElement;
const signupBtn = document.getElementById('submitBtn') as HTMLButtonElement;

const otpModal = new bootstrap.Modal(document.getElementById('otpMdl'));
const otpSubmit = document.getElementById("submitOtp") as HTMLButtonElement;
const otpVal = document.getElementById("otpVal") as HTMLInputElement; 

acInit([
        () => acFormHandler('signup-form', submitLoginForm),
        () => acFormHandler('otp-form', verifyDeets)
]);

const submitLoginForm = async function () {
        const signupdata = {
                firstName: firstName.value,
                lastName: lastName.value,
                userName: userName.value,
                eMail: emailId.value,
                password: pWord.value,
                confirmPassword: pWordConfirm.value
        }

        if (firstName.value.length <= 1) {
                acToast("error", "Username too short");
        } else if (userName.value.length < 4) {
                acToast("error", "Username too short");
        } else if (pWord.value.length <= 6) {
                acToast('error', 'password should be atleast 6 characters long')
        } else if (pWordConfirm.value != pWord.value) {
                acToast('error', 'passwords dont match');
        } else {
                postToSignUpApi(signupdata);
        }
};

async function postToSignUpApi(signupdata: any) {
        const apiUrl = '/api/account/signup';
        signupBtn.innerHTML = "Loading...";

        try {
                const response = await acPostData(apiUrl, signupdata);
                acToast(response.type, response.data);
                console.log(response);
                if (response.type === "ok") {
                        signupBtn.innerHTML = "Logging in...";
                        otpModal!.show();
                }
        } catch (error) {
                console.error('Error during login:', error);
        } finally {
                signupBtn.innerHTML = "Log In";
        }
};

const verifyDeets = async function () {

        otpSubmit.innerHTML = "loading...";
        console.log(otpVal.value);
        console.log(userName.value);

        try {
                const dt = {
                        OTP : otpVal.value.trim(),
                        UserName : userName.value.trim()
                }
                const response = await acPostData("/api/user/verification",dt);
                console.log(dt);
                console.log(response);
                if (response.type === "ok") {
                        otpModal!.hide();
                        acToast("success","user verified redirecting to login page...");
                        setTimeout(redirect,2000);
                        
                }
                else{
                        console.log("error",response.data);
                }
        } catch (error) {
                console.error('Error during login:', error);
                acToast('error','something went wrong');
        } finally {
                otpSubmit.innerHTML = "Verify";
        }
};

const redirect = () => {
        window.location.href = "/";
      };

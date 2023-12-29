import { User } from '../../Interfaces/user.interface.js';
declare const bootstrap: any;


import { acToast, acPostData, acFormHandler, acInit } from '../../global.js';

const userNameInput = document.getElementById('userName') as HTMLInputElement;
const submitBtn = document.getElementById('recSubmitBtn') as HTMLButtonElement;

//modal props
const otpModal = new bootstrap.Modal(document.getElementById('otpMdl'));
const otpSubmit = document.getElementById("submitOtp") as HTMLButtonElement;
const otpVal = document.getElementById("otpVal") as HTMLInputElement;


acInit([
    () => acFormHandler('recovery-form', submitUserName),
    () => acFormHandler('otp-form', recoverAccount)
]);


async function submitUserName() {
    if (userNameInput.value.trim().length <= 1) {
        acToast("error", "Username too short");
    } else {
        await postToLoginApi(userNameInput.value);
    }
}

async function postToLoginApi(usernameval: string) {
    try {
        const response = await acPostData('/api/account/recover', {
            userName: usernameval
        });
        console.log(response);
        acToast(response.type, response.data);

        if (response.type === "ok") {
            const lastLink: string | null = localStorage.getItem("curr_link");
            otpModal!.show();
        }
    } catch (error) {
        console.error('Error during login:', error);
        acToast('error', 'Something went wrong');
    } finally {
        submitBtn.innerHTML = "Recover";
    }
}

const recoverAccount = async function () {

    otpSubmit.innerHTML = "Wait...";
    otpSubmit.classList.add('pe-none');
    console.log(otpVal.value);
    try {
        const dt = {
            OTP: otpVal.value.trim(),
        }
        const response = await acPostData("/api/account/loginviaotp", dt);
        console.log(response);
        if (response.type === "ok") {
            otpModal!.hide();
            acToast("success", "Logging into your acc. Make sure to set your password");
            setTimeout(
                redirect,
                2000
            );
        }
        else {
            console.log("error", response.data);
            acToast("error", "Invalid OTP");
        }
    } catch (error) {
        console.error('Error during login:', error);
        acToast('error', 'something went wrong');
    } finally {
        otpSubmit.innerHTML = "Verify";
        otpSubmit.classList.remove('pe-none');
    }
};


const redirect = () => window.location.href = "/";

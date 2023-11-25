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
const passwordInput = document.getElementById('password');
const submitBtn = document.getElementById('submitBtn');
acInit([
    setupLoginForm
]);
function setupLoginForm() {
    return __awaiter(this, void 0, void 0, function* () {
        acFormHandler('login-form', submitLoginForm);
    });
}
function submitLoginForm() {
    return __awaiter(this, void 0, void 0, function* () {
        const username = userNameInput.value;
        const password = passwordInput.value;
        if (username.length <= 2) {
            acToast("error", "Username should be at least 3 characters long.");
        }
        else if (password.length < 6) {
            acToast("error", "Password should be at least 6 characters long.");
        }
        else {
            yield postToLoginApi(username, password);
        }
    });
}
function postToLoginApi(username, password) {
    return __awaiter(this, void 0, void 0, function* () {
        const apiUrl = '/api/account/login';
        const data = {
            username,
            password
        };
        submitBtn.innerHTML = "Loading...";
        try {
            const response = yield acPostData(apiUrl, data);
            acToast(response.type, response.data);
            if (response.type === "ok") {
                submitBtn.innerHTML = "Logging in...";
                const lastLink = localStorage.getItem("curr_link");
                if (lastLink) {
                    window.location.href = lastLink;
                }
                else {
                    window.location.href = "/";
                }
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

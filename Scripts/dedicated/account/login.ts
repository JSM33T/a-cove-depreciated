import { postData, showToast, validateEmail } from '../../global.js'

const chk = document.getElementById('rememberMe') as HTMLInputElement;
const userName = document.getElementById('userName') as HTMLInputElement;
const pass = document.getElementById('password') as HTMLInputElement;
const submitBtn = document.getElementById('submitBtn') as HTMLButtonElement;
console.log(chk.value + " & " + userName.value + " & " + pass.value);


    document.addEventListener('DOMContentLoaded', function () {
        const form = document.querySelector('.login-form') as HTMLFormElement;

        form.addEventListener('submit', function (event) {
            event.preventDefault();

            const loginData = {
                email: userName.value,
                pass: pass.value
            };

            console.log(loginData);
        });
    });


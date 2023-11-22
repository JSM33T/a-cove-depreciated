// import { User } from '../../Interfaces/user.interface.js';
// import { acToast, acPostData } from '../../global.js';

// const userNameInput = document.getElementById('userName') as HTMLInputElement;
// const firstName = document.getElementById('userName') as HTMLInputElement;
// const lastName = document.getElementById('userName') as HTMLInputElement;
// const eMail = document.getElementById('userName') as HTMLInputElement;
// const passWord = document.getElementById('password') as HTMLInputElement;
// const passWordConfirm = document.getElementById('password') as HTMLInputElement;
// const signupBtn = document.getElementById('submitBtn') as HTMLButtonElement;

// document.addEventListener('DOMContentLoaded', function () {
//     const form = document.querySelector('.login-form') as HTMLFormElement;
//     form.addEventListener('submit', async function (event) {
//         event.preventDefault();
//         await submitLoginForm();
//     });
// });

// async function submitLoginForm() {
//     const username = userNameInput.value;
//     const password = passwordInput.value;

//     if (username.length <= 2) {
//         acToast("error", "Username should be at least 3 characters long.");
//     } else if (password.length < 6) {
//         acToast("error", "Password should be at least 6 characters long.");
//     } else {
//         await postToLoginApi(username, password);
//     }
// }

// async function postToLoginApi(username: string, password: string) {
//     const apiUrl = '/api/account/login';
//     const data: { username: string; password: string } = { username, password };

//     signupBtn.innerHTML = "Loading...";

//     try {
//         const response = await acPostData(apiUrl, data);
//         acToast(response.type, response.data);

//         if (response.type === "ok") {
//                 signupBtn.innerHTML = "Logging in...";
//             const lastLink: string | null = localStorage.getItem("curr_link");

//             if (lastLink) {
//                 window.location.href = lastLink;
//             } else {
//                 window.location.href = "/";
//             }
//         }
//     } catch (error) {
//         console.error('Error during login:', error);
//     } finally {
//         signupBtn.innerHTML = "Log In";
//     }
// }

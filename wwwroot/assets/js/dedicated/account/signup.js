var _ref, _ref1;
function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) {
    try {
        var info = gen[key](arg), value = info.value;
    } catch (error) {
        reject(error);
        return;
    }
    info.done ? resolve(value) : Promise.resolve(value).then(_next, _throw);
}
function _async_to_generator(fn) {
    return function() {
        var self = this, args = arguments;
        return new Promise(function(resolve, reject) {
            var gen = fn.apply(self, args);
            function _next(value) {
                asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value);
            }
            function _throw(err) {
                asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err);
            }
            _next(void 0);
        });
    };
}
import { acInit, acToast, acPostData, acFormHandler } from "../../global.js";
let firstName = document.getElementById('firstName'), lastName = document.getElementById('lastName'), userName = document.getElementById('userName'), emailId = document.getElementById('emailId'), pWord = document.getElementById('pWord'), pWordConfirm = document.getElementById('pWordConfirm'), signupBtn = document.getElementById('submitBtn'), otpModal = new bootstrap.Modal(document.getElementById('otpMdl')), otpSubmit = document.getElementById("submitOtp"), otpVal = document.getElementById("otpVal");
acInit([
    ()=>acFormHandler('signup-form', submitLoginForm),
    ()=>acFormHandler('otp-form', verifyDeets)
]);
let submitLoginForm = (_ref = _async_to_generator(function*() {
    let signupdata = {
        firstName: firstName.value,
        lastName: lastName.value,
        userName: userName.value,
        eMail: emailId.value,
        password: pWord.value,
        confirmPassword: pWordConfirm.value
    };
    firstName.value.length <= 1 ? acToast("error", "Username too short") : userName.value.length < 4 ? acToast("error", "Username too short") : pWord.value.length <= 6 ? acToast('error', 'Password should be atleast 6 characters long') : pWordConfirm.value != pWord.value ? acToast('error', 'Passwords dont match') : function(signupdata) {
        return _postToSignUpApi.apply(this, arguments);
    }(signupdata);
}), function submitLoginForm() {
    return _ref.apply(this, arguments);
});
function _postToSignUpApi() {
    return (_postToSignUpApi = _async_to_generator(function*(signupdata) {
        signupBtn.innerHTML = "Wait...", signupBtn.classList.add('pe-none');
        try {
            let response = yield acPostData('/api/account/signup', signupdata);
            acToast(response.type, response.data), console.log(response), "ok" === response.type && (signupBtn.innerHTML = "Logging in...", otpModal.show());
        } catch (error) {
            console.log("typescirpt side error"), console.error('Error during login:', error);
        } finally{
            signupBtn.innerHTML = "Log In", signupBtn.classList.remove('pe-none');
        }
    })).apply(this, arguments);
}
let verifyDeets = (_ref1 = _async_to_generator(function*() {
    otpSubmit.innerHTML = "Wait...", otpSubmit.classList.add('pe-none'), console.log(otpVal.value), console.log(userName.value);
    try {
        let dt = {
            OTP: otpVal.value.trim(),
            UserName: userName.value.trim()
        }, response = yield acPostData("/api/user/verification", dt);
        console.log(dt), console.log(response), "ok" === response.type ? (otpModal.hide(), acToast("success", "user verified redirecting to login page..."), setTimeout(redirect, 2000)) : console.log("error", response.data);
    } catch (error) {
        console.error('Error during login:', error), acToast('error', 'something went wrong');
    } finally{
        otpSubmit.innerHTML = "Verify", otpSubmit.classList.remove('pe-none');
    }
}), function verifyDeets() {
    return _ref1.apply(this, arguments);
}), redirect = ()=>window.location.href = "/";

//# sourceMappingURL=signup.js.map
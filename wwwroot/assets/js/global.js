var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
function getUrl(url) {
    let targetUrl = url || window.location.href;
    let urlObject = new URL(targetUrl);
    let path = urlObject.pathname;
    return {
        path: path,
        fullUrl: targetUrl
    };
}
function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}
function acSanitize(text) {
}
function acPostData(apiUrl, data) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            if (!tokenElement) {
                return { type: 'error', message: 'Anti-forgery token element not found' };
            }
            const token = tokenElement.value;
            axios.defaults.headers.common['RequestVerificationToken'] = token;
            const response = yield axios.post(apiUrl, data);
            return { type: 'ok', data: response.data };
        }
        catch (error) {
            if (error) {
                return { type: 'error', data: error.response.data };
            }
            else {
                return { type: 'error', data: "Something went wrong" };
            }
        }
    });
}
function acGetData(apiUrl) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            if (!tokenElement) {
                return { type: 'error', message: 'Anti-forgery token element not found' };
            }
            const token = tokenElement.value;
            axios.defaults.headers.common['RequestVerificationToken'] = token;
            const response = yield axios.get(apiUrl);
            return { type: 'ok', data: response.data };
        }
        catch (error) {
            if (error) {
                return { type: 'error', data: error.response.data };
            }
            else {
                return { type: 'error', data: "Something went wrong" };
            }
        }
    });
}
function classesToTags(tag, classes) {
    var elements = document.getElementsByTagName(tag);
    if (elements.length > 0) {
        var classesArray = classes.split(' ');
        for (var i = 0; i < elements.length; i++) {
            for (var j = 0; j < classesArray.length; j++) {
                elements[i].classList.add(classesArray[j]);
            }
        }
        console.log("Classes added successfully!");
    }
    else {
        console.error("No elements with tag '" + tag + "' found.");
    }
}
function acInit(functions) {
    if (document.readyState === "complete") {
        functions.forEach(func => func());
    }
    else {
        document.addEventListener('DOMContentLoaded', () => {
            functions.forEach(func => func());
        });
    }
}
function acSetEvent(trigger, target) {
    const commentButton = document.getElementById(trigger);
    if (commentButton) {
        commentButton.addEventListener('click', target);
    }
}
function acTemplate(templateId, data, divid) {
    const templateContainer = document.getElementById(templateId);
    if (templateContainer) {
        const template = templateContainer.innerHTML;
        const renderedTemplate = template.replace(/\{\{(\w+)\}\}/g, (match, variable) => {
            return data[variable] || match;
        });
        const target = document.getElementById(divid);
        if (target) {
            target.innerHTML = renderedTemplate;
        }
    }
    else {
        console.error("template rendering failed");
    }
}
function getQueryParameters() {
    var queryString = window.location.search;
    var params = {};
    if (queryString) {
        var queryParams = queryString.substring(1).split('&');
        queryParams.forEach(function (param) {
            var pair = param.split('=');
            var key = decodeURIComponent(pair[0]);
            var value = decodeURIComponent(pair[1] || '');
            if (params[key]) {
                if (Array.isArray(params[key])) {
                    params[key].push(value);
                }
                else {
                    params[key] = [params[key], value];
                }
            }
            else {
                params[key] = value;
            }
        });
    }
    return params;
}
function acToast(type, message) {
    let toastContainer = document.getElementById('toast-container');
    const toastElement = document.createElement('div');
    toastElement.classList.add('toast');
    toastElement.setAttribute('role', 'alert');
    toastElement.setAttribute('autohide', 'false');
    toastElement.setAttribute('aria-live', 'assertive');
    toastElement.setAttribute('aria-atomic', 'true');
    const toastHeader = document.createElement('div');
    toastHeader.classList.add('toast-header', 'bg-primary', 'text-white');
    const strong = document.createElement('strong');
    strong.classList.add('mr-auto');
    strong.textContent = type;
    toastHeader.appendChild(strong);
    const closeBtn = document.createElement('button');
    closeBtn.type = 'button';
    closeBtn.classList.add('btn-close', 'btn-close-white', 'ms-2');
    closeBtn.setAttribute('data-dismiss', 'toast');
    closeBtn.setAttribute('aria-label', 'Close');
    closeBtn.setAttribute('data-bs-dismiss', 'toast');
    closeBtn.innerHTML = '<span aria-hidden="true">&times;</span>';
    toastHeader.appendChild(closeBtn);
    const toastBody = document.createElement('div');
    toastBody.classList.add('toast-body');
    toastBody.textContent = message;
    toastElement.appendChild(toastHeader);
    toastElement.appendChild(toastBody);
    toastContainer.appendChild(toastElement);
    const toast = new bootstrap.Toast(toastElement);
    toast.show();
    toastElement.addEventListener('hidden.bs.toast', function () {
        toastContainer.removeChild(toastElement);
    });
}
function shareIt() {
}
function acQueryParams(key, value) {
    let currentUrl = new URL(window.location.href);
    if (currentUrl.searchParams.has(key)) {
        currentUrl.searchParams.set(key, value);
    }
    else {
        currentUrl.searchParams.append(key, value);
    }
    history.pushState({}, '', currentUrl.href);
}
function acClearParams() {
}
function acFormHandler(formId, submitMethod) {
    const form = document.getElementById(formId);
    if (form) {
        form.addEventListener('submit', (event) => __awaiter(this, void 0, void 0, function* () {
            event.preventDefault();
            yield submitMethod(event);
        }));
    }
    else {
    }
}
class Url {
    constructor(url) {
        const targetUrl = url || window.location.href;
        this.urlObject = new URL(targetUrl);
    }
    get protocol() {
        return this.urlObject.protocol;
    }
    get host() {
        return this.urlObject.host;
    }
    get hostname() {
        return this.urlObject.hostname;
    }
    get port() {
        return this.urlObject.port;
    }
    get path() {
        return this.urlObject.pathname;
    }
    get query() {
        return this.urlObject.search;
    }
    get hash() {
        return this.urlObject.hash;
    }
    get fullUrl() {
        return this.urlObject.href;
    }
}
export { acInit, acTemplate, acSetEvent, acQueryParams, acClearParams, getQueryParameters, acGetData, acPostData, acFormHandler, validateEmail, classesToTags, getUrl, acToast, shareIt, Url };

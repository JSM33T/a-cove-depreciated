import { Email } from './Interfaces/email.interface';
declare const axios: any;
declare const AxiosResponse: any;
declare const bootstrap: any;

///===================== URL UTILS ====================

function getUrl(url?: string) {
    let targetUrl: string = url || window.location.href;

    let urlObject = new URL(targetUrl);
    let path = urlObject.pathname;

    return {
        path: path,
        fullUrl: targetUrl
    };
}

//===================== VALIDATIONS ====================
//email validation
function validateEmail(email: string): boolean {
    const emailRegex: RegExp = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function acSanitize(text: string) {

}

async function acPostData(apiUrl, data) {
    try {
        const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
        if (!tokenElement) {
            return { type: 'error', message: 'Anti-forgery token element not found' };
        }
        const token = tokenElement.value;
        axios.defaults.headers.common['RequestVerificationToken'] = token;
        const response = await axios.post(apiUrl, data);
        return { type: 'ok', data: response.data };
    } catch (error: any) {
        if (error) {
            return { type: 'error', data: error.response.data };
        }
        else {
            return { type: 'error', data: "Something went wrong" };
        }
    }
}


async function acGetData(apiUrl) {
    try {
        const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
        if (!tokenElement) {
            return { type: 'error', message: 'Anti-forgery token element not found' };
        }
        const token = tokenElement.value;
        axios.defaults.headers.common['RequestVerificationToken'] = token;
        const response = await axios.get(apiUrl);
        return { type: 'ok', data: response.data };
    } catch (error: any) {
        if (error) {
            return { type: 'error', data: error.response.data };
        }
        else {
            return { type: 'error', data: "Something went wrong" };
        }
    }
}
//async function acGetData(apiUrl: string): Promise<{ success: boolean; data?: any; error?: string }> {
//    try {
//        const response = await axios.get(apiUrl);
//        return { success: true, data: response.data };
//    } catch (error: any) {
//        console.error('Error:', error);

//        // Adjust the error handling as needed
//        const errorMessage = error.response?.data?.message || 'An error occurred';
//        return { success: false, error: errorMessage };
//    }
//}


//function getData(apiUrl): Promise<string> {
//    return new Promise<string>(async (resolve, reject) => {
//        try {
//            const token = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
//            axios.defaults.headers.common['RequestVerificationToken'] = token.value;
//            const response = await axios.get(apiUrl);
//            resolve(response.data.toString());
//        } catch (error: any) {
//            if (error.response && error.response.data) {
//                resolve(error.response.data.toString());
//            } else if (error.request) {
//                resolve('No response received from the server');
//            } else {
//                resolve('Error sending API request');
//            }
//        }
//    });
//}



//===================== HTML UTILS ====================

function classesToTags(tag, classes) {
    // Get all elements with the specified tag name
    var elements = document.getElementsByTagName(tag);

    // Check if elements were found
    if (elements.length > 0) {
        // Split the classes string into an array based on spaces
        var classesArray = classes.split(' ');

        // Iterate through each element and add the classes
        for (var i = 0; i < elements.length; i++) {
            for (var j = 0; j < classesArray.length; j++) {
                elements[i].classList.add(classesArray[j]);
            }
        }

        console.log("Classes added successfully!");
    } else {
        console.error("No elements with tag '" + tag + "' found.");
    }
}

//function classesToTags(tag, classes) {
//    // Get all elements with the specified tag name
//    var elements = document.getElementsByTagName(tag);

//    // Check if elements were found
//    if (elements.length > 0) {
//        // Split the classes string into an array
//        var classesArray = classes.split('');

//        // Iterate through each element and add the classes
//        for (var i = 0; i < elements.length; i++) {
//            for (var j = 0; j < classesArray.length; j++) {
//                elements[i].classList.add(classesArray[j]);
//            }
//        }

//        console.log("Classes added successfully!");
//    } else {
//        console.error("No elements with tag '" + tag + "' found.");
//    }
//}


//===================== XHR VIA AXIOS ====================



function acSetEvent(trigger, target) {
    const commentButton = document.getElementById(trigger);
    if (commentButton) {
        commentButton.addEventListener('click', target);
    }

}

// function acInit(functionNames: string[]): void {
//    document.addEventListener('DOMContentLoaded', () => {
//        functionNames.forEach(functionName => {
//            const func = window[functionName];
//            if (typeof func === 'function') {
//                const funcSignature = (func as () => void);
//                funcSignature();
//            } else {
//                console.error(`Function ${functionName} is not defined or is not a function.`);
//            }
//        });
//    });
//}
function acInit(functions: (() => void)[]): void {
    document.addEventListener('DOMContentLoaded', () => {
        functions.forEach(func => func());
    });
}


function acTemplate(templateId: any, data: JSON, divid: string) {
    const templateContainer = document.getElementById(templateId);
    if (templateContainer) {
        const template = templateContainer.innerHTML;
        const renderedTemplate = template.replace(/\{\{(\w+)\}\}/g, (match, variable) => {
            // Use the data if available, otherwise keep the placeholder
            return data[variable] || match;
        });
        // Insert the rendered template into the container
        const target = document.getElementById(divid);
        if (target) { target.innerHTML = renderedTemplate; }

    }
    else {
        console.error("template rendering failed")
    }



    // Replace variables in the template with actual data

}


function getQueryParameters() {
    // Get the query string from the URL
    var queryString = window.location.search;

    // Initialize an object to store the parameters and their values
    var params = {};

    // Check if there are any query parameters
    if (queryString) {
        // Remove the leading "?" and split the remaining string into key-value pairs
        var queryParams = queryString.substring(1).split('&');

        // Iterate through each key-value pair
        queryParams.forEach(function (param) {
            var pair = param.split('=');

            // Decode the key and value, and add them to the params object
            var key = decodeURIComponent(pair[0]);
            var value = decodeURIComponent(pair[1] || '');

            // If the key already exists in the object, convert it to an array
            if (params[key]) {
                if (Array.isArray(params[key])) {
                    params[key].push(value);
                } else {
                    params[key] = [params[key], value];
                }
            } else {
                params[key] = value;
            }
        });
    }

    return params;
}


//============global ac methods=============


function acToast(type: string, message: string) {

    let toastContainer = document.getElementById('toast-container');

    // Create a new toast element using the DOM API
    const toastElement = document.createElement('div');
    toastElement.classList.add('toast');
    toastElement.setAttribute('role', 'alert');
    toastElement.setAttribute('autohide', 'false');
    toastElement.setAttribute('aria-live', 'assertive');
    toastElement.setAttribute('aria-atomic', 'true');

    // Create toast header
    const toastHeader = document.createElement('div');
    toastHeader.classList.add('toast-header','bg-primary','text-white');

    //const i = document.createElement('i');
    //i.classList.add('ai-bell', 'fs-lg me-2');
    //toastHeader.appendChild(i);

    const strong = document.createElement('strong');
    strong.classList.add('mr-auto');
    strong.textContent = type;
    toastHeader.appendChild(strong);

    // Close button with data-bs-dismiss attribute
    const closeBtn = document.createElement('button');
    closeBtn.type = 'button';
    closeBtn.classList.add('btn-close', 'btn-close-white', 'ms-2');
    closeBtn.setAttribute('data-dismiss', 'toast');
    closeBtn.setAttribute('aria-label', 'Close');
    closeBtn.setAttribute('data-bs-dismiss', 'toast'); // This line closes modals
    closeBtn.innerHTML = '<span aria-hidden="true">&times;</span>';
    toastHeader.appendChild(closeBtn);

    // Create toast body
    const toastBody = document.createElement('div');
    toastBody.classList.add('toast-body');
    toastBody.textContent = message;

    // Append header and body to the toast element
    toastElement.appendChild(toastHeader);
    toastElement.appendChild(toastBody);

    // Append the toast element to the body
   // document.body.appendChild(toastElement);
    toastContainer!.appendChild(toastElement);

    const toast = new bootstrap.Toast(toastElement);
    toast.show();

    toastElement.addEventListener('hidden.bs.toast', function () {
        toastContainer!.removeChild(toastElement);
    });
}



function shareIt() {
    // share using apis
}



function acQueryParams(key: string, value: string) {
    let currentUrl = new URL(window.location.href);
    // Check if the query parameter is already present
    if (currentUrl.searchParams.has(key)) {
        // If yes, update the existing value
        currentUrl.searchParams.set(key, value);
    } else {
        // If not, add a new query parameter
        currentUrl.searchParams.append(key, value);
    }
    // Update the browser URL
    history.pushState({}, '', currentUrl.href);
}
function acClearParams() {
}



export {
    //ac methods
    //
    acInit, acTemplate, acSetEvent,
    //manage query param states
    acQueryParams, acClearParams,
    //getter and poster currently axios
    acGetData, acPostData,

    validateEmail,
    classesToTags,
    getQueryParameters,
    getUrl,
    acToast,
    shareIt
}
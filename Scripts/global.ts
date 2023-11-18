declare const axios: any;


//===================== VALIDATIONS ====================
//email validation
function validateEmail(email: string): boolean {
    const emailRegex: RegExp = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function acSanitize(text: string)
{

}

//===================== XHR VIA AXIOS ====================
function postData(apiUrl, ipData): Promise<string> {
    return new Promise<string>(async (resolve, reject) => {
        try {
            const token = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
            axios.defaults.headers.common['RequestVerificationToken'] = token.value;
            const response = await axios.post(apiUrl, ipData);
            resolve(response.data.toString());
        } catch (error: any) {
            if (error.response && error.response.data) {
                resolve(error.response.data.toString());
            } else if (error.request) {
                resolve('No response received from the server');
            } else {
                resolve('Error sending API request');
            }
        }
    });
}



function getData(apiUrl): Promise<string> {
    return new Promise<string>(async (resolve, reject) => {
        try {
            const token = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
            axios.defaults.headers.common['RequestVerificationToken'] = token.value;
            const response = await axios.get(apiUrl);
            resolve(response.data.toString());
        } catch (error: any) {
            if (error.response && error.response.data) {
                resolve(error.response.data.toString());
            } else if (error.request) {
                resolve('No response received from the server');
            } else {
                resolve('Error sending API request');
            }
        }
    });
}


//===================== XHR VIA AXIOS ====================

function toaster(type: string, msg: string)
{
    alert(type + " :- " + msg);
}


function acSetEvent(trigger, target)
{
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
        if (target)
        { target.innerHTML = renderedTemplate;  }
        
    }
    else
    {
        console.error("template rendering failed")
    }



    // Replace variables in the template with actual data
   
}


export {
    validateEmail,
    postData,
    acSetEvent,
    acInit,
    acTemplate,
    toaster

}
 function validateEmail(email: string): boolean {
    const emailRegex: RegExp = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

declare const axios: any;
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

export {
    validateEmail,
    postData
}
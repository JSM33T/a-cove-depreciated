declare function getUrl(url?: string): {
    path: string;
    fullUrl: string;
};
declare function validateEmail(email: string): boolean;
declare function acPostData(apiUrl: string, data: any): Promise<{
    type: string;
    message: string;
    data?: undefined;
} | {
    type: string;
    data: any;
    message?: undefined;
}>;
declare function acGetData(apiUrl: string): Promise<{
    type: string;
    message: string;
    data?: undefined;
} | {
    type: string;
    data: any;
    message?: undefined;
}>;
declare function classesToTags(tag: string, classes: string): void;
declare function acInit(functions: (() => void)[]): void;
declare function acSetEvent(trigger: string, target: (this: HTMLElement, ev: MouseEvent) => any): void;
declare function acTemplate(templateId: any, data: JSON, divid: string): void;
declare function getQueryParameters(): {};
declare function acToast(type: string, message: string): void;
declare function shareIt(): void;
declare function acQueryParams(key: string, value: string): void;
declare function acClearParams(): void;
declare function acFormHandler(formId: string, submitMethod: (event: Event) => Promise<void>): void;
declare class Url {
    private urlObject;
    constructor(url?: string);
    get protocol(): string;
    get host(): string;
    get hostname(): string;
    get port(): string;
    get path(): string;
    get query(): string;
    get hash(): string;
    get fullUrl(): string;
}
export { acInit, acTemplate, acSetEvent, acQueryParams, acClearParams, getQueryParameters, acGetData, acPostData, acFormHandler, validateEmail, classesToTags, getUrl, acToast, shareIt, Url };

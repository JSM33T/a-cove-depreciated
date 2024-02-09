export interface IProfile {
    avatarId: number;
    avatarImg: string;
    badges: null | string[]; // Assuming badges can be an array of strings or null
    bio: string;
    confirmPassword: null | string; // Assuming confirmPassword can be a string or null
    dateEdited: string;
    dateElement: string;
    dateJoined: string;
    eMail: string;
    firstName: string;
    gender: string; // Assuming gender is a string with values 'm' or 'f'
    id: number;
    lastName: string;
    password: null | string; // Assuming password can be a string or null
    phone: string;
    role: string;
    userName: string;
}

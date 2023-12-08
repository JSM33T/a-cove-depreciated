
import { acGetData, acInit, prettifyDate } from '../../global.js'
import { IProfile } from '../../Interfaces/profile.interface.js';

const Name = document.getElementById("fullName") as HTMLSpanElement;
const Email = document.getElementById("emailId") as HTMLSpanElement;
const Bio = document.getElementById("bio") as HTMLSpanElement;
const Gender = document.getElementById("gender") as HTMLSpanElement;
const DateJoined = document.getElementById("dateJoined") as HTMLSpanElement;
const UserName = document.getElementById("userName") as HTMLSpanElement;
const Avatar = document.getElementById("avatarPlaceHolder") as HTMLElement;


acInit([
    fetchDetails
])


async function fetchDetails() {
    const response = acGetData('api/profile/getdetails');
    console.log(response);
    const resp: IProfile = (await response).data;
    Name.innerHTML = resp.firstName + ' ' + resp.lastName;
    Bio.innerHTML = resp.bio;
    Email.innerHTML = resp.eMail;
    Gender.innerHTML = resp.gender === "m" ? "Male" : resp.gender === "f" ? "Female" : "Other";
    DateJoined.innerHTML = prettifyDate(resp.dateElement);
    UserName.innerHTML = "@" + resp.userName.trim();
    Avatar.style.backgroundImage = 'url(/assets/images/avatars/default/' + resp.avatarImg + '.png)'

}

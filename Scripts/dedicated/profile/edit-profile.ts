
import { acFormHandler, acGetData, acInit, acPostData, prettifyDate } from '../../global.js'
import { IProfile } from '../../Interfaces/profile.interface.js';

const firstName = document.getElementById("firstName") as HTMLInputElement;
const lastName = document.getElementById("lastName") as HTMLInputElement;
const emailId = document.getElementById("emailId") as HTMLInputElement;
const userName = document.getElementById("userName") as HTMLInputElement;
const bio = document.getElementById("bio") as HTMLTextAreaElement;

const Avatar = document.getElementById("avatarPlaceHolder") as HTMLElement;
const avatarDdl = document.getElementById("avatarDdl") as HTMLSelectElement;


acInit([
    fetchDetails,
    loadavatarDdl,
    onAvatarChangeEvent,
    () => acFormHandler('basicInfoForm', submitDetails),
    () => acFormHandler('passForm', submitPass)
])


async function onAvatarChangeEvent() {
    avatarDdl!.addEventListener("change", function () {
        var selectedOption = avatarDdl!.options[avatarDdl!.selectedIndex];
        var imgValue = selectedOption.dataset.img;
        Avatar!.style.backgroundImage = 'url(/assets/images/avatars/default/' + imgValue + '.png)';
    });
}

async function fetchDetails() {
    const response = acGetData('/api/profile/getdetails');
    
    const resp: IProfile = (await response).data;
    console.log(resp);
    firstName.value = resp.firstName;
    lastName.value = resp.lastName;
    userName.value = resp.userName.trim();
    emailId.value = resp.eMail;
    bio.value = resp.bio;
    avatarDdl.value = resp.avatarId.toString().trim();
    Avatar.style.backgroundImage = 'url(/assets/images/avatars/default/' + resp.avatarImg + '.png)'
    console.log(avatarDdl.value = resp.avatarId.toString().trim());
}

async function loadavatarDdl() {
    const optns = await acGetData("/api/getavatars");
    let options = ` <option value="" selected>Select avatar</option>`;
    var i;
    for (i = 1; i < optns.data.length; i++) {
        options += `<option value="${optns.data[i].id}" data-img="${optns.data[i].image}">${optns.data[i].title}</option>`;
    }
    avatarDdl.innerHTML = options;
}

async function submitDetails() {
    const data = {
        firstName: firstName.value,
        lastName: lastName.value,
        bio: bio.value,
        avatarId: parseInt(avatarDdl.value) || 0,
        eMail: emailId.value,
        gender: "m",
        userName: userName.value
    }
    const resp = acPostData('/api/profile/update', data)
}


async function submitPass() {
}

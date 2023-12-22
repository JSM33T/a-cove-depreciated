
import { acFormHandler, acGetData, acInit, acPostData, acToast } from '../../global.js'
import { IProfile } from '../../Interfaces/profile.interface.js';

const firstName = document.getElementById("firstName") as HTMLInputElement;
const lastName = document.getElementById("lastName") as HTMLInputElement;
const emailId = document.getElementById("emailId") as HTMLInputElement;
const userName = document.getElementById("userName") as HTMLInputElement;
const bio = document.getElementById("bio") as HTMLTextAreaElement;

// const detailsForm = document.getElementById("basicInfoForm") as HTMLFormElement;
// const passForm = document.getElementById("passForm") as HTMLFormElement;

const Avatar = document.getElementById("avatarPlaceHolder") as HTMLElement;
const avatarDdl = document.getElementById("avatarDdl") as HTMLSelectElement;


acInit([
    onAvatarChangeEvent,
    async () => {
        await loadavatarDdl();
        await fetchDetails();
    },
    async () => acFormHandler('basicInfoForm', submitDetails),
    async () => acFormHandler('passForm', submitPass)
])


async function onAvatarChangeEvent() {

    avatarDdl!.addEventListener("change", function () {

        var selectedOption = avatarDdl!.options[avatarDdl!.selectedIndex];

        var imgValue = selectedOption.dataset.img;

        Avatar!.style.backgroundImage = 'url(/assets/images/avatars/default/' + imgValue + '.png)';
    });
}


async function getCheckedRadioButton() {
    //var genderRadios = document.getElementsByName('gender');

    //for (var i = 0; i < genderRadios.length; i++) {
    //    if (genderRadios[i].checked) {
    //        return genderRadios[i];
    //    }
    //}
    //return null;
    return "m";
}

async function fetchDetails() {
    const response = acGetData('/api/profile/getdetails');
    const resp: IProfile = (await response).data;
    firstName.value = resp.firstName;
    lastName.value = resp.lastName;
    userName.value = resp.userName;
    emailId.value = resp.eMail;
    bio.value = resp.bio;
    avatarDdl.value = resp.avatarId!.toString();
    Avatar.style.backgroundImage = 'url(/assets/images/avatars/default/' + resp.avatarImg + '.png)';
    console.log(response);
}
    
async function loadavatarDdl() {
    const optns = await acGetData("/api/getavatars");
    let options = ` <option value="" selected disabled>Select avatar</option>`;
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
        avatarId: parseInt(avatarDdl.value) || 1,
        eMail: emailId.value,
        gender: "m",
        userName: userName.value
    }
    const resp = await acPostData('/api/profile/update', data);
        if(resp.type == "ok")
        {
            acToast("Success", resp.data);
            await updatePfp();

        }
        else
        {
            acToast("error",resp.data);
        }    
}

async function updatePfp() {

    var selectedOption = avatarDdl.options[avatarDdl.selectedIndex];
    var dataImgValue = selectedOption.getAttribute("data-img");

    console.log(dataImgValue);
    
    var imgElements = document.getElementsByClassName("avatar-placeholder") as HTMLCollectionOf<HTMLImageElement>;

    for (var i = 0; i < imgElements.length; i++) {

        imgElements[i].src = "/assets/images/avatars/default/" + dataImgValue + ".png";
        console.log(imgElements[i].src);
    }


}

async function submitPass() {
    const newpass = document.getElementById('new-pass') as HTMLInputElement;
    const confirmpass = document.getElementById('confirm-pass') as HTMLInputElement;
    if (newpass.value == "" || newpass.value.length < 6) {
        acToast('validation issue', 'password too short');
        return
    }
    if (newpass.value != confirmpass.value) {
        acToast('error', 'Passwords don\'t match');
        return;
    }

    const data = {
        password: newpass.value
    };
    const res = await acPostData('/api/profile/password/update', data);
    console.log(res);
    if (res.type == "ok") {
        acToast('success', res.data);
    }
    else {
        acToast('error', res.data);
    }
}
async function logOutFromAll() {
    const resp = acGetData('/api/account/disposekey');
    console.log(resp);
}
async function clearPreferences() {
}
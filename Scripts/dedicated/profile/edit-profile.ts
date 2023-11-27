
import { acFormHandler, acGetData, acInit,acPostData,prettifyDate } from '../../global.js'
import { Profile } from '../../Interfaces/profile.interface.js';

const firstName = document.getElementById("firstName") as HTMLInputElement;
const lastName = document.getElementById("lastName") as HTMLInputElement;
const emailId = document.getElementById("emailId") as HTMLInputElement;
const userName = document.getElementById("userName") as HTMLInputElement;
const bio = document.getElementById("bio") as HTMLTextAreaElement;

const detailsForm = document.getElementById("basicInfoForm") as HTMLFormElement;
const passForm = document.getElementById("passForm") as HTMLFormElement;

const Avatar = document.getElementById("avatarPlaceHolder") as HTMLElement;
const avatarDdl = document.getElementById("avatarDdl") as HTMLSelectElement;



acInit([
    fetchDetails,
    loadavatarDdl,
    onAvatarChangeEvent,
    () => acFormHandler('detailsForm', submitDetails),
    () => acFormHandler('detailsForm', submitPass)
])


async function onAvatarChangeEvent() {
 

    // Attach the event listener to the dropdown
    //avatarDdl.addEventListener("change", function () {
        
    //    Avatar.style.backgroundImage = 'url(/assets/images/avatars/default/' + avatarDdl.dataset.img + '.png)' 
    //});


    avatarDdl!.addEventListener("change", function () {
        // Get the selected option
        var selectedOption = avatarDdl!.options[avatarDdl!.selectedIndex];

        // Get the value of the data-img attribute
        var imgValue = selectedOption.dataset.img;

        // Update the background image of the Avatar element
        
        Avatar!.style.backgroundImage = 'url(/assets/images/avatars/default/' + imgValue + '.png)';
    });


}

async function fetchDetails()
{
    const response = acGetData('/api/profile/getdetails');
    const resp: Profile = (await response).data;
    firstName.value = resp.firstName;
    lastName.value = resp.lastName;
    userName.value = resp.userName.trim();
    emailId.value = resp.eMail;
    bio.value = resp.bio;
    avatarDdl.value = resp.avatarId.toString();
    Avatar.style.backgroundImage = 'url(/assets/images/avatars/default/'+ resp.avatarImg +'.png)' 

}

async function loadavatarDdl() {
    const optns = await acGetData("/api/getavatars");
    let options = ` <option value="" selected disabled>Select avatar</option>`;
    var i;
    for (i = 1; i < optns.data.length; i++)
    {
        options += `<option value="${optns.data[i].id}" data-img="${optns.data[i].image}">${optns.data[i].title}</option>`;
    }                     
    avatarDdl.innerHTML = options;
}

async function submitDetails() {
    const data = {
        firstName : "",
        lastName : ""

    }
    const resp = acPostData('ssss', data)
}


async function submitPass(){
}
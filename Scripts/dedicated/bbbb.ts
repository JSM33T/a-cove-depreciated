import { toaster, acInit, classesToTags, getQueryParameters, showToast } from './../global.js'


const btbt = document.getElementById("falana") as HTMLButtonElement;
if (btbt) {
    btbt.addEventListener('click', changeQuery);
}

function changeQuery() {
    // Get the current query parameters
    var currentParams = new URLSearchParams(window.location.search);

    // Add or modify a parameter
    currentParams.set('exampleParam', 'exampleValue');

    // Get the new query string
    var newQueryString = currentParams.toString();

    // Change the URL without triggering a page reload
    history.pushState({}, '', window.location.pathname + '?' + newQueryString);

    // Log the updated URL
    console.log('Updated URL:', window.location.href);
    handleQueryChange();
}
function handleQueryChange() {
    // Get the current query string
    var currentQueryString = window.location.search;

    // Compare with the previous query string (you may want to store the initial value)
    if (currentQueryString !== handleQueryChange.lastQueryString) {
        // Query string has changed, perform your desired actions
        console.log('Query string changed:', currentQueryString);

        // Update the lastQueryString for the next comparison
        handleQueryChange.lastQueryString = currentQueryString;
    }
    var queryParams = getQueryParameters();
    console.log(queryParams);
}

window.onload = function () {
    handleQueryChange.lastQueryString = window.location.search;
    handleQueryChange();
};

// Initial setup: Call the function to store the initial query string
handleQueryChange.lastQueryString = window.location.search;

// Attach the event listener to detect changes
window.onpopstate = handleQueryChange;


console.log("hey thee");

showToast('error','finally toast is working!');


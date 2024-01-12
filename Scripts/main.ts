import { acGetData, acInit, fetchJsonFile, Url } from './global.js'
declare const bootstrap: any;
declare const axios: { get: (arg0: string) => Promise<{ data: string | any[]; }>; }

const shareBtn = document.getElementById('share-btn') as HTMLButtonElement;
const lsearch = document.getElementById('global_search') as HTMLInputElement;

acInit([
    () => shareBtn.addEventListener('click', shareIt),
    () => lsearch.addEventListener('keyup', livesearch),
    getUpdates
]);


async function getUpdates() {
    try {
        const updatesData = await fetchJsonFile<any>('/store/updates.json');
        const updatePlaceholder = document.getElementById('updates-placeholder') as HTMLSpanElement;
        if (updatesData && updatesData.updates) {
            const updateEntries = updatesData.updates;

            for (const entry of updateEntries) {
                if (entry.link.length == 0 || entry.link.length == "") {
                    updatePlaceholder.innerHTML = updatePlaceholder.innerHTML + `
                    <li> <span>${entry.type} ${entry.title}</span></li>
                `;
                }
                else {
                    updatePlaceholder.innerHTML = updatePlaceholder.innerHTML + `
                    ${entry.type}:
                    <li><a href="${entry.link}"><span> ${entry.title}</span></a></li>
                `;
                }
                
            }
        } else {
            console.error('Invalid or missing changelog data');
        }
    } catch (error) {
        console.error('Error fetching or processing changelog:', error);
    }
}

async function shareIt() {
    const currentUrl = new Url();
    const linkholder = document.getElementById('link-placeholder') as HTMLElement;
    linkholder.innerHTML = currentUrl.fullUrl;

    const copyBtn = document.getElementById('copy-btn') as HTMLButtonElement;
    copyBtn.addEventListener('click', () => {
        navigator.clipboard.writeText(linkholder.innerHTML)
        copyBtn.innerHTML = '<i class="ai-check me-2 ms-n1"></i>Copied!'
        setTimeout(() => {
            copyBtn.innerHTML = '<i class="ai-copy me-2 ms-n1"></i>Copy'
        }, 2000)
    })
}

async function livesearch() {

    const searchStat = document.getElementById('search_stat');
    const searchResults = document.getElementById('search_results');
    if (lsearch.value.length >= 2) {
        const response = await acGetData('/api/liversearch/all/' + lsearch.value);
        if (response.type == 'error') {
            return;
        }
        let sb = "";
        console.log(response);
        for (let i = 0; i < response.data.length; i++) {
            sb += `
            <div class="d-flex align-items-center border-bottom pb-4 mb-4 fade-in">
                <a class="position-relative d-inline-block flex-shrink-0 bg-secondary rounded-1" href="${response.data[i].url}">
                <img src="/assets/images/search_thumbs/${response.data[i].image}.svg" width="70" alt="Product" /></a>
                <div class="ps-3">
                    <h4 class="h6 mb-2"><a href="${response.data[i].url}">${response.data[i].title}</a></h4>
                    <span class="fs-sm text-muted ms-auto">${response.data[i].description}</span>'
                </div>
            </div>
            `;
        }
        searchResults!.innerHTML = sb.toString();
        searchStat!.innerHTML = 'Search';
    }
    else {
        searchResults!.innerHTML = '';
        searchStat!.innerHTML = 'Search';
    }
    

    //if (lsearch.value.length >= 2) {
    //    axios.get('/api/livesearch/all/' + lsearch.value)
    //        .then((response: { data: string | any[]; }) => {
    //            console.log(response);
    //            var sb = "";
    //            for (var i = 0; i < response.data.length; i++) {
    //                sb += `
    //                <div class="d-flex align-items-center border-bottom pb-4 mb-4 fade-in">
    //                    <a class="position-relative d-inline-block flex-shrink-0 bg-secondary rounded-1" href="${response.data[i].url}">
    //                    <img src="/assets/images/search_thumbs/${response.data[i].image}.svg" width="70" alt="Product" /></a>
    //                    <div class="ps-3">
    //                        <h4 class="h6 mb-2"><a href="${response.data[i].url}">${response.data[i].title}</a></h4>
    //                        <span class="fs-sm text-muted ms-auto">${response.data[i].description}</span>'
    //                    </div>
    //                </div>
    //                `;
    //            }
    //            searchResults!.innerHTML = sb.toString();
    //            searchStat!.innerHTML = 'Search';
    //        })
    //        .catch(error => {
    //            console.error('Error:', error);
    //            searchStat!.innerHTML = '';
    //            searchResults!.innerHTML = '';
    //        });
    //}
    //else {
    //    searchResults!.innerHTML = '';
    //    searchStat!.innerHTML = 'Search';
    //}
}

import { acInit } from './global.js';
acInit([
    searchEvent,
    shareEvent
]);
function searchEvent() { }
function shareEvent() { }
function livesearch() {
    const searchStat = document.getElementById('search_stat');
    const searchResults = document.getElementById('search_results');
    var ddr = document.getElementById("global_search");
    if (ddr.value.length >= 2) {
        axios.get('/api/livesearch/all/' + ddr)
            .then((response) => {
            console.log(response.data);
            var sb = "";
            for (var i = 0; i < response.data.length; i++) {
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
            searchResults.innerHTML = sb.toString();
            searchStat.innerHTML = 'Search';
        })
            .catch(error => {
            console.error('Error:', error);
            searchStat.innerHTML = '';
            searchResults.innerHTML = '';
        });
    }
    else {
        searchResults.innerHTML = '';
        searchStat.innerHTML = 'Search';
    }
}
//# sourceMappingURL=main.js.map
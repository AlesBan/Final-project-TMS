document.addEventListener('DOMContentLoaded', ( e) => {
    document.getElementById('open-search-form').addEventListener('click',
        (e) => {
            document.getElementById('open-dropdown').classList.add('dropdown-content-show')
        });
});
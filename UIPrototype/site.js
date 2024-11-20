// Original source: https://reintech.io/blog/implementing-infinite-scroll-bootstrap-5
function loadMoreContent(shouldIncludeLoadMoreButton) 
{
    if (document.getElementById('LoadMoreButton')) { document.getElementById('LoadMoreButton').remove(); }

    TopicIDSelected = document.getElementById('TopicSelection').options[document.getElementById('TopicSelection').selectedIndex].id;

    // TODO: The service URL will have to come from the back-end
    fetch('https://localhost:7006/api/FlashCards?TopicID=' + TopicIDSelected + '&NumberOfFlashcards=3')
        .then(response => response.text())
        .then(html => 
        {
            document.getElementById('FlashCardContainer').innerHTML += html; 
            if (shouldIncludeLoadMoreButton) { document.getElementById('FlashCardContainer').innerHTML += '<button id="LoadMoreButton" onclick="loadMoreContentViaButtonClick()">Load more</button>'; }
        });
}
const loadInitialContent = () => {
    document.getElementById('FlashCardContainer').innerHTML = "";
    loadMoreContent(true);
}
const loadMoreContentViaButtonClick = () => loadMoreContent(true);
const loadMoreContentViaScrolling = () => loadMoreContent(false);


window.addEventListener('scroll', function() 
{
    var scrollableContent = document.getElementById('FlashCardContainer');
    var hasReachedBottom = scrollableContent.getBoundingClientRect().bottom <= window.innerHeight;

    if (hasReachedBottom) {
        loadMoreContentViaScrolling();
    }
});
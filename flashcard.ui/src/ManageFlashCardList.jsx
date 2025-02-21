import { useState, useEffect } from "react";


const ManageFlashCardList = (props) =>
{
    // TODO: Fix the name of the variable here; it should be be camelcase
    const [flashcards, setFlashCards] = useState([]);

    const fetchMostRecentFlashCards = async (numberOfCards) => {

        try {
            // TODO: Uncomment this
            //setLoadingState(LoadingStatus.isLoading);

            const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCards?TopicID=" + props.topicID + "&NumberOfFlashcards=" + numberOfCards + "&OrderBy=-CreatedOn");

            const rawFlashCards = await response.json();

            // If there is an error in the service response, rawFlashCards won't be an array, which means the .map() function will fail.  I have to check the type here.
            if (rawFlashCards instanceof Array) {

                // If the function should reset the list (i.e., because the topic was changed), then just set the flashcards
                // property to the new list.  Otherwise, append the new list to the existing one (using the Javascript
                // spread operators on both).
                //
                // This conditional is necessary to avoid a race condition.  Previously I was trying to clear out the
                // flashcards property using setFlashCards([]) and then calling fetchMoreFlashCards.  Since both of those
                // modified the flashcards property, the first one never fully executed, so new topic's cards were getting
                // appended to the existing list.  Merging the logic into one method that takes a boolean avoids this
                // because everything is done here, once.
                setFlashCards(rawFlashCards);

                // TODO: Uncomment this
                //setLoadingState(LoadingStatus.loaded);
            }

        }
        catch (ex) {
            // TODO: Uncomment this
            // TODO: Log the error
            //setLoadingState(LoadingStatus.hasError);
        }

    };


    const loadInitialSetForThisTopic = async () => await fetchMostRecentFlashCards(20);


    // When the selected topic changes, clear out the list and start fresh
    useEffect(() => {
        loadInitialSetForThisTopic();
    }, [props.topicID]);


    const deleteFlashCard = async (flashcardID) => {
        // TODO: Wrap this in a Try...Catch
        const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCard?PartitionKey=" + props.topicID + "&FlashCardID=" + flashcardID, { method: 'DELETE' });

        if (response.ok) {
            loadInitialSetForThisTopic();

            // TODO: How can I get the component to re-render based on the change to the flashcards array without doing a roundtrip to the API?
            //       The below code works to update remove the item just deleted from the local flashcards array, but since nothing
            //       in the ManageFlashCardList component has flashcards in a dependency array, I can't get the component
            //       to re-render.
            // Adapted from: https://stackoverflow.com/questions/5767325/how-can-i-remove-a-specific-item-from-an-array-in-javascript
            // const indexOfFlashCardToRemove = flashcards.indexOf(flashcards.find(fc => fc.id == flashcardID));
            // if (indexOfFlashCardToRemove > -1) {
            //     flashcards.splice(indexOfFlashCardToRemove, 1);
            //     setFlashCards(flashcards);
            // }
        } else {
            window.alert("An error occurred deleting this flashcard: HTTP Response Code " + response.status);
        }
    };


    const handleDeleteFlashCard = (question, flashcardID) => {
        if (window.confirm("Are you sure you want to delete the flashcard '" + question + "'?")) {
            deleteFlashCard(flashcardID);
        }
    };


    return (
        <>
            <link href="/css/ManageFlashCardList.css" rel="stylesheet" />
            <div id="FlashCardContainer">
                {flashcards.map((fc, index) => (
                    <div className="flashCardManagementContainer" key={index} id={index}>
                        <div className="flashCardQAndA">
                            <div><strong>Q:</strong> {fc.question}</div>
                            <div><strong>A:</strong> {fc.answer}</div>
                        </div>
                        <div className="deleteButton">
                            <button onClick={() => handleDeleteFlashCard(fc.question, fc.id) }>
                                <img src="/images/bin.png" width="25px" />
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </>
    );
};


export default ManageFlashCardList;
import { useState, useEffect } from "react";


const ManageFlashCardList = (props) =>
{
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
                        <div className="deleteButton"><img src="/images/bin.png" width="25px" /></div>
                    </div>
                ))}
            </div>
        </>
    );
};


export default ManageFlashCardList;
import { useState, useEffect } from "react";


const ManageFlashCardList = (props) =>
{
    // TODO: Fix the name of the variable here; it should be be camelcase
    const [flashcards, setFlashCards] = useState([]);
    const [flashCard, setFlashCard] = useState({ question: "", answer: "", topic: { id: "" } });

    const fetchMostRecentFlashCards = async (numberOfCards) => {

        try {
            // TODO: Uncomment this
            //setLoadingState(LoadingStatus.isLoading);

            const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCards?TopicID=" + props.topicID + "&NumberOfFlashcards=" + numberOfCards + "&OrderBy=-CreatedOn");

            const rawFlashCards = await response.json();

            // If there is an error in the service response, rawFlashCards won't be an array, which means the .map() function will fail.  I have to check the type here.
            if (rawFlashCards instanceof Array) {
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


    // TODO: This needs to be wired up to the form
    // Capture all changes to the properties in the flashCard state object
    const propertyChange = ((e) => setFlashCard({ ...flashCard, [e.target.name]: e.target.value }));


    const addNewFlashCard = async () => {
        // TODO: Wrap this in a Try...Catch

        const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCard",
            {
                method: 'POST',
                body: JSON.stringify(flashCard),
                headers:
                {
                    'Content-Type': 'application/json'
                }
            });

        if (response.ok) {
            loadInitialSetForThisTopic();
        } else {
            window.alert("An error occurred deleting this flashcard: HTTP Response Code " + response.status);
        }
    };

    const resetForm = () => {
        flashCard.question = "";
        flashCard.answer = "";
        flashCard.topic.id = "";
        setFlashCard(flashCard);
    };

    const saveFlashCardHandler = (e) => {
        e.preventDefault();

        // Validate the form
        if (!flashCard.question || !flashCard.answer) {
            alert("Please enter something for all required fields.");
            return;
        }

        flashCard.topic.id = props.topicID;
        // TODO: This is a cheat.  TopicName is required by the API, but is not actually used by the app.  I only pass the ID to the component, so if
        //       I wanted to get the name, I'd have to make another API call to look it up, or modify the component to take both the ID and the name.
        //       For now, setting this to an underscore satisfies all of the code, but logically it's clunky.
        flashCard.topic.topicName = "_";
        setFlashCard(flashCard);

        addNewFlashCard();
        resetForm();
    };


    return (
        <>
            <link href="/css/ManageFlashCardList.css" rel="stylesheet" />

            <form onSubmit={saveFlashCardHandler}>
                Question*: <input type="text" value={flashCard.question} onChange={(e) => setFlashCard({ ...flashCard, question: e.target.value })} />
                Answer*: <input type="text" value={flashCard.answer} onChange={(e) => setFlashCard({ ...flashCard, answer: e.target.value })} />
                <input type="submit" value="Save" />
            </form>

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
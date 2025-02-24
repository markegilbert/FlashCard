import { useState, useEffect } from "react";
import LoadingStatus from "../Helpers/LoadingStatus";
import LoadingIndicator from "./LoadingIndicator";


const ManageFlashCardList = (props) =>
{
    const [flashCards, setFlashCards] = useState([]);
    const [flashCard, setFlashCard] = useState({ question: "", answer: "", topic: { id: "" } });
    const [errorMessage, setErrorMessage] = useState("");
    const [loadingState, setLoadingState] = useState(LoadingStatus.isLoading);


    // TODO: Remove this when done testing
    //const delay = ms => new Promise(res => setTimeout(res, ms));


    const fetchMostRecentFlashCards = async (numberOfCards) => {

        try {
            setLoadingState(LoadingStatus.isLoading);

            // TODO: Remove this when done testing
            //await delay(2000);

            const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCards?TopicID=" + props.topicID + "&NumberOfFlashcards=" + numberOfCards + "&OrderBy=-CreatedOn");

            const rawFlashCards = await response.json();

            // If there is an error in the service response, rawFlashCards won't be an array, which means the .map() function will fail.  I have to check the type here.
            if (rawFlashCards instanceof Array) {
                setFlashCards(rawFlashCards);

                setLoadingState(LoadingStatus.loaded);
            }

        }
        catch (ex) {
            // TODO: Log the error
            setErrorMessage("An error occurred retrieving the flashcards for this topic.");
        }

    };


    const loadInitialSetForThisTopic = async () => await fetchMostRecentFlashCards(20);


    // When the selected topic changes, clear out the list and start fresh
    useEffect(() => {
        loadInitialSetForThisTopic();
    }, [props.topicID]);


    const handleDeleteFlashCard = async (question, flashcardID) => {
        if (window.confirm("Are you sure you want to delete the flashcard '" + question + "'?")) {
            const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCard?PartitionKey=" + props.topicID + "&FlashCardID=" + flashcardID, { method: 'DELETE' });

            if (response.ok) {
                loadInitialSetForThisTopic();

                // TODO: How can I get the component to re-render based on the change to the flashcards array without doing a
                //       roundtrip to the API?  The below code works to update remove the item just deleted from the local
                //       flashcards array, but since nothing in the ManageFlashCardList component has flashcards in a
                //       dependency array, I can't get the component to re-render.
                // Adapted from: https://stackoverflow.com/questions/5767325/how-can-i-remove-a-specific-item-from-an-array-in-javascript
                // const indexOfFlashCardToRemove = flashcards.indexOf(flashcards.find(fc => fc.id == flashcardID));
                // if (indexOfFlashCardToRemove > -1) {
                //     flashcards.splice(indexOfFlashCardToRemove, 1);
                //     setFlashCards(flashcards);
                // }

                setErrorMessage("");
            } else {
                setErrorMessage("An error occurred deleting this flashcard: HTTP Response Code " + response.status);
            }
        }
    };


    // Persist all changes to the e.target property to the flashCard state object
    const handlePropertyChange = ((e) => {
        setFlashCard({ ...flashCard, [e.target.name]: e.target.value })
    });


    const resetForm = () => {
        flashCard.question = "";
        flashCard.answer = "";
        flashCard.topic.id = "";
        setFlashCard(flashCard);
    };

    const handleAddNewFlashCard = async (e) => {
        e.preventDefault();

        // Validate the form
        if (!flashCard.question || !flashCard.answer) {
            setErrorMessage("Please enter something for all required fields.");
            return;
        }

        flashCard.topic.id = props.topicID;
        // TODO: This is a cheat.  TopicName is required by the API, but is not actually used by the app.  I only pass the ID to the component, so if
        //       I wanted to get the name, I'd have to make another API call to look it up, or modify the component to take both the ID and the name.
        //       For now, setting this to an underscore satisfies all of the code, but logically it's clunky.
        flashCard.topic.topicName = "_";
        setFlashCard(flashCard);


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
            resetForm();
            setErrorMessage("");
        } else {
            setErrorMessage("An error occurred deleting this flashcard: HTTP Response Code " + response.status);
        }
    };


    const FlashCardList = () => {
        if (loadingState != LoadingStatus.loaded) { return null; }

        return (
            <div id="FlashCardContainer">
                {flashCards.map((fc, index) => (
                    <div className="flashCardManagementContainer" key={index} id={index}>
                        <div className="flashCardQAndA">
                            <div><strong>Q:</strong> {fc.question}</div>
                            <div><strong>A:</strong> {fc.answer}</div>
                        </div>
                        <div className="deleteButton">
                            <button onClick={() => handleDeleteFlashCard(fc.question, fc.id)}>
                                <img src="/images/bin.png" width="25px" />
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        );
    };


    return (
        <>
            <link href="/css/ManageFlashCardList.css" rel="stylesheet" />

            <div className="errorMessage">{errorMessage}</div>

            <div className={loadingState == LoadingStatus.loaded ? "addFlashCardForm" : "hideContainer"}>
                <form onSubmit={handleAddNewFlashCard}>
                    <div className="formInstructionsText">Fields denoted with * are required.</div>
                    <div>
                        <div className="inputLabel">Question *:</div>
                        <div><textarea value={flashCard.question} name="question" onChange={(e) => handlePropertyChange(e)} className="formTextArea" /></div>
                    </div>
                    <div>
                        <div className="inputLabel">Answer *:</div>
                        <div><textarea value={flashCard.answer} name="answer" onChange={(e) => handlePropertyChange(e)} className="formTextArea" /></div>
                    </div>
                    <input type="submit" value="Add New" />
                </form>
            </div>

            <LoadingIndicator loadingState={loadingState} hasErrorMessage="There was a problem retrieving the list of flashcards.  Please verify that the services are running." />
            <FlashCardList />
        </>
    );
};


export default ManageFlashCardList;
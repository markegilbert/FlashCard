import { useEffect, useState, useContext } from "react";
import LoadingStatus from "../Helpers/LoadingStatus";
import LoadingIndicator from "./LoadingIndicator";
import FlashCardContext from "../Helpers/FlashCardContext";


const FlashCardList = (props) => {
    const [flashcards, setFlashCards] = useState([]);
    const [loadingState, setLoadingState] = useState(LoadingStatus.isLoading);

    // TODO: Document this
    const { currentNavLocation, currentTopicId, navigate } = useContext(FlashCardContext);

    const fetchMoreFlashCards = async (shouldResetListFirst, numberOfCards) => {

        try {
            setLoadingState(LoadingStatus.isLoading);

            //const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCards?TopicID=" + props.topicID + "&NumberOfFlashcards=" + numberOfCards);
            const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/FlashCards?TopicID=" + currentTopicId + "&NumberOfFlashcards=" + numberOfCards);

            const rawFlashCards = await response.json();

            // If there is an error in the service response, rawFlashCards won't be an array, which means the .map() function will fail.  I have to check the type here.
            if (rawFlashCards instanceof Array) {
                // Add the showQuestion property to the array elements, defaulted to true so that the new questions show up by default
                const enrichedFlashCards = rawFlashCards.map(obj => ({ ...obj, showQuestion: true }));


                // If the function should reset the list (i.e., because the topic was changed), then just set the flashcards
                // property to the new list.  Otherwise, append the new list to the existing one (using the Javascript
                // spread operators on both).
                //
                // This conditional is necessary to avoid a race condition.  Previously I was trying to clear out the
                // flashcards property using setFlashCards([]) and then calling fetchMoreFlashCards.  Since both of those
                // modified the flashcards property, the first one never fully executed, so new topic's cards were getting
                // appended to the existing list.  Merging the logic into one method that takes a boolean avoids this
                // because everything is done here, once.
                if (shouldResetListFirst) { setFlashCards(enrichedFlashCards); }
                else { setFlashCards([...flashcards, ...enrichedFlashCards]); }

                setLoadingState(LoadingStatus.loaded);
            }

        }
        catch (ex) {
            // TODO: Log the error
            setLoadingState(LoadingStatus.hasError);
        }

    };

    // TODO: What should be the source of these numbers?  Front-end config?
    const loadInitialSetForTopic = async () => await fetchMoreFlashCards(true, 10);
    const loadMoreForThisTopic = async () => await fetchMoreFlashCards(false, 5);


    // When the topic changes, clear out the list and start fresh
    useEffect(() => {
        loadInitialSetForTopic();
    }, [props.topicID]);


    // When the user gets to the bottom of the existing list of flashcards, load more
    // Adapted from https://builtin.com/articles/react-infinite-scroll
    useEffect(() => {

        const handleScroll = () => {
            const { scrollTop, clientHeight, scrollHeight } = document.documentElement;
            if (scrollTop + clientHeight >= scrollHeight - 100) {
                loadMoreForThisTopic();
            }
        };

        window.addEventListener("scroll", handleScroll);


        // This is the cleanup portion of the hook, and is used when the component is unmounted.
        return () => {
            window.removeEventListener("scroll", handleScroll);
        };
    }, [flashcards]);


    const ToggleFlashCardSideVisibility = (fcIndex) => {
        const updatedFlashCardList = [...flashcards];
        updatedFlashCardList[fcIndex.index].showQuestion = !updatedFlashCardList[fcIndex.index].showQuestion;
        setFlashCards(updatedFlashCardList);
    };

    const ShowLoadMoreButton = () => {
        if (loadingState != LoadingStatus.loaded || flashcards.length == 0) { return null; }
        else { return <button className="loadMoreControl" onClick={loadMoreForThisTopic}>Load More</button>; }
    }

    const FlashCardList = () => {
        if (loadingState != LoadingStatus.loaded) { return null; }

        if (flashcards.length == 0) { return <p>No flashcards have been defined for this topic yet</p>; }

        return (
            <div id="FlashCardContainer" className="container flashCardContainer">
                {flashcards.map((fc, index) => (
                    <div className={fc.showQuestion ? 'flashCard flashCardQuestionShowing' : 'flashCard'} key={index} id={index} onClick={() => ToggleFlashCardSideVisibility({ index })}>
                        <div className={fc.showQuestion ? 'flashCardQuestion' : 'flashCardQuestion flashCardHiddenSide'}>{fc.question}</div>
                        <div className={fc.showQuestion ? 'flashCardAnswer flashCardHiddenSide' : 'flashCardAnswer'}>{fc.answer}</div>
                    </div>
                ))}
            </div>
        );
    }


    return (
        <>
            <link href="/css/FlashCard.css" rel="stylesheet" />
            <LoadingIndicator loadingState={loadingState} hasErrorMessage="There was a problem retrieving the list of flashcards.  Please verify that the services are running." />
            <FlashCardList />
            <ShowLoadMoreButton />
        </>
    )

}

export default FlashCardList;
import { useEffect, useState } from "react";

const FlashCardList = (props) => {
    const [flashcards, setFlashCards] = useState([]);
    const [hasServiceError, setHasServiceError] = useState(false);


    const fetchMoreFlashCards = async (shouldResetListFirst, numberOfCards) => {

        try {
            // TODO: The URL, and possible the entire string should come from the config
            const response = await fetch("https://localhost:7006/api/FlashCards?TopicID=" + props.topicID + "&NumberOfFlashcards=" + numberOfCards);
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
            }

            setHasServiceError(false);
        }
        catch (ex) {
            // TODO: Log the error
            setHasServiceError(true);
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
            if (scrollTop + clientHeight >= scrollHeight - 20) {
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

    const ShowError = () => {
        if (hasServiceError) { return <p>There was a problem retrieving the list of flashcards.  Please verify that the services are running.</p>; }
        else { return null; }
    }

    const ShowSpinner = () => {
        if (hasServiceError) { return null; }
        else { <img src="./images/loading.gif" width="100px" /> }
    }


    return (
        <>
            <ShowError />

            <div id="FlashCardContainer" className="container flashCardContainer">
                {flashcards.map((fc, index) => (
                    <div className={fc.showQuestion ? 'flashCard flashCardQuestionShowing' : 'flashCard'} key={index} id={index} onClick={() => ToggleFlashCardSideVisibility({ index })}>
                        <div className={fc.showQuestion ? 'flashCardQuestion' : 'flashCardQuestion flashCardHiddenSide'}>{fc.question}</div>
                        <div className={fc.showQuestion ? 'flashCardAnswer flashCardHiddenSide' : 'flashCardAnswer'}>{fc.answer}</div>
                    </div>
                ))}
            </div>

            <ShowSpinner />
        </>
    )

}

export default FlashCardList;
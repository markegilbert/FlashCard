import { useEffect, useState } from "react";

const FlashCardList = () => {
    const [flashcards, setFlashCards] = useState([]);


    // Adapted from https://builtin.com/articles/react-infinite-scroll
    useEffect(() => {
        const fetchFlashCards = async () => {
            // TODO: This should come from the back-end
            const response = await fetch("https://localhost:7006/api/FlashCards?TopicID=MovieQuestions&NumberOfFlashcards=3");
            //const response = await fetch("https://localhost:7006/api/FlashCards?TopicID=MontyPythonQuestions&NumberOfFlashcards=3");
            const rawFlashCards = await response.json();

            // Add the showQuestion property to the array elements, defaulted to true so that the question shows up by default
            const enrichedFlashCards = rawFlashCards.map(obj => ({ ...obj, showQuestion: true }));

            // Combine the new array with the existing one, using the Javascript spread operators with both
            setFlashCards([...flashcards, ...enrichedFlashCards]);
        };

        const handleScroll = () => {
            const { scrollTop, clientHeight, scrollHeight } = document.documentElement;
            if (scrollTop + clientHeight >= scrollHeight - 20) {
                fetchFlashCards();
            }
        };

        window.addEventListener("scroll", handleScroll);
        window.addEventListener("load", handleScroll);
        document.getElementById("AddMoreButton").addEventListener("click", handleScroll);

        return () => {
            // This is the cleanup portion of the hook, and is used when the component is unmounted.
            window.removeEventListener("scroll", handleScroll);
            window.removeEventListener("load", handleScroll);
            document.getElementById("AddMoreButton").removeEventListener("click", handleScroll);
        };
    }, [flashcards]);


    const ToggleFlashCardSideVisibility = (fcIndex) => {
        const updatedFlashCardList = [...flashcards];
        updatedFlashCardList[fcIndex.index].showQuestion = !updatedFlashCardList[fcIndex.index].showQuestion;
        setFlashCards(updatedFlashCardList);
    };




    return (
        <>
            <link href="/css/FlashCard.css" rel="stylesheet" />

            <div id="FlashCardContainer" className="container flashCardContainer">
                {flashcards.map((fc, index) => (
                    <div className={fc.showQuestion ? 'flashCard flashCardQuestionShowing' : 'flashCard'} key={index} id={index} onClick={() => ToggleFlashCardSideVisibility({ index })}>
                        <div className={fc.showQuestion ? 'flashCardQuestion' : 'flashCardQuestion flashCardHiddenSide' }>{fc.question}</div>
                        <div className={fc.showQuestion ? 'flashCardAnswer flashCardHiddenSide' : 'flashCardAnswer'}>{fc.answer}</div>
                    </div>
                ))}
            </div>

            <button id="AddMoreButton">Add more</button>
        </>
    );


}

export default FlashCardList;
import { useEffect, useState } from "react";

const FlashCardList = () => {
    const [flashcards, setFlashCards] = useState([]);


    useEffect(() => {
        const fetchFlashCards = async () => {
            // TODO: This should come from the back-end
            const response = await fetch("https://localhost:7006/api/FlashCards?TopicID=MovieQuestions&NumberOfFlashcards=3");
            const rawFlashCards = await response.json();
            //setFlashCards([...flashcards,
            //                   rawFlashCards]);
            setFlashCards(rawFlashCards);
        }

        fetchFlashCards();
    }, []);


    return (
        <>
            <link href="/css/FlashCard.css" rel="stylesheet" />

            <div id="FlashCardContainer" className="container">
                {flashcards.map(fc => <p key={fc.id}>{fc.question}</p>) }
            </div>
        </>
    );
}

export default FlashCardList;
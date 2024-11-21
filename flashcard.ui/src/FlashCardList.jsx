import { useEffect, useState } from "react";

const FlashCardList = () => {
    const [flashcards, setFlashCards] = useState([]);


    useEffect(() => {
        const fetchFlashCards = async () => {
            // TODO: This should come from the back-end
            const response = await fetch("https://localhost:7006/api/FlashCards?TopicID=MovieQuestions&NumberOfFlashcards=3");
            const rawFlashCards = await response.json();

            // Add the showQuestion property to the array elements, defaulted to true so that the question shows up by default
            const enrichedFlashCards = rawFlashCards.map(obj => ({ ...obj, showQuestion: true}));

            //setFlashCards([...flashcards,
            //                   rawFlashCards]);
            setFlashCards(enrichedFlashCards);
        }

        fetchFlashCards();
    }, []);


    const ToggleFlashCardSideVisibility = (fcIndex) => {
        const updatedFlashCardList = [...flashcards];
        updatedFlashCardList[fcIndex.index].showQuestion = !updatedFlashCardList[fcIndex.index].showQuestion;
        setFlashCards(updatedFlashCardList);
    }

    return (
        <>
            <link href="/css/FlashCard.css" rel="stylesheet" />

            <div id="FlashCardContainer" className="container">
                {flashcards.map((fc, index) => (
                    <div className="flashCard" key={index} id={index} onClick={() => ToggleFlashCardSideVisibility({ index })}>
                        <div className={fc.showQuestion ? 'flashCardQuestion' : 'flashCardQuestion flashCardHiddenSide' }>{fc.question}</div>
                        <div className={fc.showQuestion ? 'flashCardAnswer flashCardHiddenSide' : 'flashCardAnswer'}>{fc.answer}</div>
                    </div>
                ))}
            </div>
        </>
    );
}

export default FlashCardList;
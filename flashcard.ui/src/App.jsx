import TopicSelection from "./TopicSelection"
import FlashCardList from "./FlashCardList"
import { useState } from "react";
import ErrorBoundary from "./ErrorBoundary"


function App() {
    const [topicSelected, setTopicSelection] = useState("");

    return (
        <>
            <link href="/css/App.css" rel="stylesheet" />
            <ErrorBoundary fallback="<p>An error occurred loading the topics list.  Please check that the service is running.</p>">
                <TopicSelection onSelectTopic={(topicSelected) => setTopicSelection(topicSelected)} />
            </ErrorBoundary>
            <ErrorBoundary fallback="<p>Could not load the flashcards.  Please check that the service is running.</p>">
                <FlashCardList topicID={topicSelected} />
            </ErrorBoundary>
        </>
    )
}

export default App

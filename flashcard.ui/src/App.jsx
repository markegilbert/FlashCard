import TopicSelection from "./TopicSelection"
import FlashCardList from "./FlashCardList"
import { useState } from "react";


function App() {
    const [topicSelected, setTopicSelection] = useState("");

    return (
        <>
            <link href="/css/App.css" rel="stylesheet" />
            <TopicSelection onSelectTopic={(topicSelected) => setTopicSelection(topicSelected)}/>
            <FlashCardList topicID={topicSelected} />
        </>
    )
}

export default App

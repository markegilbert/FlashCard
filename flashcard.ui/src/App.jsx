import TopicSelection from "./TopicSelection"
import FlashCardList from "./FlashCardList"
import { useState } from "react";
import Navbar from "./Navbar";
import NavigationProvider from "../Helpers/NavigationProvider";


const App = () => {
    // TODO: I think this can be removed once I have the navigationContext up and running
    const [topicSelected, setTopicSelection] = useState("");


    return (
        <NavigationProvider>
            <link href="/css/App.css" rel="stylesheet" />

            <TopicSelection onSelectTopic={(topicSelected) => setTopicSelection(topicSelected)} />
            <Navbar />
            <FlashCardList topicID={topicSelected} />
        </NavigationProvider>
    )
}

export default App;


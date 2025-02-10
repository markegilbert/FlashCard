import TopicSelection from "./TopicSelection"
import FlashCardList from "./FlashCardList"
import { useState, useCallback, createContext } from "react";
import navValues from "../Helpers/navValues";
import Navbar from "./Navbar";


const navigationContext = createContext(navValues.study);

function App() {
    // TODO: I think this can be removed once I have the navigationContext up and running
    const [topicSelected, setTopicSelection] = useState("");

    const navigate = useCallback(
        (navTo) => setNav({ current: navTo, navigate }),
        []
    );

    //const [nav, setNav] = useState({ current: navValues.study, navigate });
    const [nav, setNav] = useState(navValues.study);


    return (
        <navigationContext.Provider value={nav}>
            <link href="/css/App.css" rel="stylesheet" />

            {/*<TopicSelection onSelectTopic={(topicSelected) => setTopicSelection(topicSelected)} />*/}
            {/*<Navbar currentNavLocation={nav} />*/}
            <Navbar currentNavLocation="Study" />
            {/*<FlashCardList topicID={topicSelected} />*/}
        </navigationContext.Provider>
    )
}

export { navigationContext };
export default App

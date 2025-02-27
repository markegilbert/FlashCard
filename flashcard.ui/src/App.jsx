import TopicSelection from "./TopicSelection"
import Navbar from "./Navbar";
import FlashCardContextProvider from "../Helpers/FlashCardContextProvider";
import ViewSelector from "./ViewSelector";


const App = () => {
    return (
        <FlashCardContextProvider>
            <link href="/css/App.css" rel="stylesheet" />

            <div className="sticky-div">
                <TopicSelection />
                <Navbar />
            </div>

            <ViewSelector />
        </FlashCardContextProvider>
    )
}

export default App;


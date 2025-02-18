import { useEffect, useState, useContext } from "react";
import LoadingStatus from "../Helpers/LoadingStatus";
import LoadingIndicator from "./LoadingIndicator";
import FlashCardContext from "../Helpers/FlashCardContext";


const TopicSelection = () => {
    const [topics, setTopics] = useState([]);
    const [topicID, setTopicID] = useState("");
    const [loadingState, setLoadingState] = useState(LoadingStatus.isLoading);


    // Destructure only the properties from FlashCardContext that I need in this component
    const { currentNavLocation, navigate } = useContext(FlashCardContext);


    const updateSelectedTopic = (selectedTopicID) => {
        setTopicID(selectedTopicID);
        navigate(currentNavLocation, selectedTopicID);
    };


    useEffect(() => {
        const fetchTopics = async () => {

            try {
                setLoadingState(LoadingStatus.isLoading);

                const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/Topics");
                const rawTopics = await response.json();

                // TODO: Verify that rawTopics is an array
                setTopics(rawTopics);

                // Default the list to the first topic
                if (rawTopics && rawTopics.length > 0) {
                    updateSelectedTopic(rawTopics[0].id);
                }

                setLoadingState(LoadingStatus.loaded);
            }
            catch (ex) {
                // TODO: Log the error
                setLoadingState(LoadingStatus.hasError);
            }
        }

        fetchTopics();
    }, []);


    const handleChange = (selectedID) => {
        updateSelectedTopic(selectedID);
    };


    const TopicSelector = () => {

        if (topics.length == 0) { return null; }
        else {
            return (
                <div className="sticky-div">
                    <select id="TopicSelection" onChange={(e) => handleChange(e.target.options[e.target.selectedIndex].id)} value={topicID}>
                        {topics.map(t => <option key={t.id} id={t.id} value={t.id}>{t.topicName}</option>)}
                    </select>
                </div>
            );
        }
    };

    return (
        <>
            <link href="/css/TopicSelection.css" rel="stylesheet" />
            <LoadingIndicator loadingState={loadingState} hasErrorMessage="There was a problem loading the list of available topics.  Please verify that the services are running." />
            <TopicSelector />
        </>
    );
}


export default TopicSelection;
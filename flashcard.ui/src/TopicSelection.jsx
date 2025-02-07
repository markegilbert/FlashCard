import { useEffect, useState } from "react";
import LoadingStatus from "../Helpers/LoadingStatus";
import LoadingIndicator from "./LoadingIndicator";


const TopicSelection = (props) => {
    const [topics, setTopics] = useState([]);
    const [topicID, setTopicID] = useState("");
    const [loadingState, setLoadingState] = useState(LoadingStatus.isLoading);


    const updateSelectedTopicProp = (selectedTopicID) => {
        props.onSelectTopic(selectedTopicID);
        setTopicID(selectedTopicID);
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
                    updateSelectedTopicProp(rawTopics[0].id);
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
        updateSelectedTopicProp(selectedID);
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
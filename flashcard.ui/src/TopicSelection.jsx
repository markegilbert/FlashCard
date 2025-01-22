import { useEffect, useState } from "react";


const TopicSelection = (props) => {
    const [topics, setTopics] = useState([]);
    const [hasServiceError, setHasServiceError] = useState(false);
    const [topicID, setTopicID] = useState("");


    const updateSelectedTopicProp = (selectedTopicID) => {
        props.onSelectTopic(selectedTopicID);
        setTopicID(selectedTopicID);
    };


    useEffect(() => {
        const fetchTopics = async () => {

            try {
                const response = await fetch(import.meta.env.FLASHCARD_SERVICE_BASE_URL + "/api/Topics");
                const rawTopics = await response.json();

                // TODO: Verify that rawTopics is an array
                setTopics(rawTopics);

                setHasServiceError(false);

                // Default the list to the first topic
                if (rawTopics && rawTopics.length > 0) {
                    updateSelectedTopicProp(rawTopics[0].id);
                }
            }
            catch (ex) {
                // TODO: Log the error
                setHasServiceError(true);
            }
        }

        fetchTopics();
    }, []);


    const handleChange = (selectedID) => {
        updateSelectedTopicProp(selectedID);
    };


    const TopicSelector = () => {
        if (hasServiceError) {
            return <p>There was a problem loading the list of available topics.  Please verify that the services are running.</p>;
        }
        else if (topics.length == 0) {
            return null;
        }
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
            <TopicSelector />
        </>
    );
}


export default TopicSelection;
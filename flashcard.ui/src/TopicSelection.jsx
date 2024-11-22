import { useEffect, useState } from "react";


const TopicSelection = (props) => {
    const [topics, setTopics] = useState([]);
    const [hasServiceError, setHasServiceError] = useState(false);

    useEffect(() => {
        const fetchTopics = async () => {

            try {
                // TODO: This should come from the back-end
                const response = await fetch("https://localhost:7006/api/Topics");
                const rawTopics = await response.json();

                // TODO: Verify that rawTopics is an array
                setTopics(rawTopics);

                setHasServiceError(false);
            }
            catch (ex) {
                // TODO: Log the error
                setHasServiceError(true);
            }
        }

        fetchTopics();
    }, []);


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
                    <select id="TopicSelection" onChange={(e) => props.onSelectTopic(e.target.options[e.target.selectedIndex].id)}>
                        {topics.map(t => <option key={t.id} id={t.id}>{t.topicName}</option>)}
                    </select>
                </div>
            );
        }
    };

    return (
        <>
            <link href="/css/App.css" rel="stylesheet" />

            <TopicSelector />
        </>
    );
}


export default TopicSelection;
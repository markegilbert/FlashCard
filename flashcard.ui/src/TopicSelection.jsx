import { useEffect, useState } from "react";


const TopicSelection = (props) => {
    const [topics, setTopics] = useState([]);

    useEffect(() => {
        const fetchTopics = async () => {
            // TODO: This should come from the back-end
            const response = await fetch("https://localhost:7006/api/Topics");
            const rawTopics = await response.json();
            setTopics(rawTopics);
        }

        fetchTopics();
    }, []);

    return (
        <>
            <link href="/css/App.css" rel="stylesheet" />

            <div className="sticky-div">
                <select id="TopicSelection" onChange={(e) => props.onSelectTopic(e.target.options[e.target.selectedIndex].id)}>
                    {topics.map(t => <option key={t.id} id={t.id}>{t.topicName}</option>)}
                </select>
            </div>
        </>
    );
}


export default TopicSelection;
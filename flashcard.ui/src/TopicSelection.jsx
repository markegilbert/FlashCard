import { useEffect, useState } from "react";
// TODO: The sticky-div class should be moved to a TopicSelection.css file


const TopicSelection = () => {
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
                <select id="TopicSelection">
                    {topics.map(t => <option key={t.id} id={t.id}>{t.topicName}</option>)}
                </select>
            </div>
        </>
    );
}


export default TopicSelection;
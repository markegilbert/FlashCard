import navValues from "../helpers/NavValues";
import { useContext } from "react";
import NavigationContext from "../Helpers/NavigationContext";
// TODO: Add the imports for addFlashCard and deleteFlashCard


const Navbar = () => {

	// Destructure the navigate function out of the context object
	const { currentNavLocation, navigate } = useContext(NavigationContext);

	switch (currentNavLocation) {
		case navValues.study:
			return (
				<>
					<link href="/css/Navbar.css" rel="stylesheet" />
					<div className="navbar">
						<ul>
							<li><div>Study</div></li>
							<li><a href="#" onClick={() => navigate(navValues.manage)}>Manage</a></li>
						</ul>
					</div>
				</>
			);
		case navValues.manage:
			return (
				<>
					<link href="/css/Navbar.css" rel="stylesheet" />
					<div className="navbar">
						<ul>
							<li><a href="#" onClick={() => navigate(navValues.study)}>Study</a></li>
							<li><div>Manage</div></li>
						</ul>
					</div>
				</>
			);
		default:
			return (
				<span>No component for navigation value &quot;{currentNavLocation}&quot; found.</span>
			);
	}
};


export default Navbar;

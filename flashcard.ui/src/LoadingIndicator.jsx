import LoadingStatus from "../Helpers/LoadingStatus";

const LoadingIndicator = ({ loadingState, hasErrorMessage }) => {
    if (loadingState == LoadingStatus.isLoading) { return <h3>Loading...</h3>; }
    if (loadingState == LoadingStatus.hasError) { return <h3>{hasErrorMessage}</h3>; }
    return null;
};

LoadingIndicator.defaultProps = {
    loadingState: LoadingStatus.loaded,
    hasErrorMessage: "An error occurred. Please try again later."
};

export default LoadingIndicator;
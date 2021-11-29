function metrica(metric) {
    const element = (
        <h2>It is {new Date().toLocaleTimeString()}.</h2>
            )
         ReactDOM.render(element, document.getElementById('root'));
}

setInterval(tick, 1000);

class metrica extends React.Component {
    render() {
        return <h2>metrica</h2>;
    }
}
ReactDOM.render(<metrica />, document.getElementById("root"));

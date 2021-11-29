class Activity extends React.Component {

    constructor(props) {
        super(props);
        this.state = { data: props.activity };
        this.onClick = this.onClick.bind(this);
    }
    onClick(e) {
        this.props.onRemove(this.state.data);
    }
    render() {
        return <div>
            <p><b>{this.state.data.name}</b></p>
            <p>Цена {this.state.data.price}</p>
            <p><button onClick={this.onClick}>Удалить</button></p>
        </div>;
    }
}

class Form extends React.Component {

    constructor(props) {
        super(props);
        this.state = { name: "", price: "" };
        this.onSubmit = this.onSubmit.bind(this);
        this.onNameChange = this.onNameChange.bind(this);
        this.onPriceChange = this.onPriceChange.bind(this);
    }
    onNameChange(e) {
        this.setState({ name: e.target.value });
    }
    onPriceChange(e) {
        this.setState({ price: e.target.value });
    }
    onSubmit(e) {
        e.preventDefault();
        var Name = this.state.name.trim();
        var Price = this.state.price;
        if (!Name || Price == "") {
            return;
        }
        this.props.onSubmit({ name: Name, price: Price });
        this.setState({ name: "", price: "" });
    }
    render() {
        return (
            <form onSubmit={this.onSubmit}>
                <p>
                    <input type="number"
                        placeholder="User Id"
                        value={this.state.name}
                        onChange={this.onNameChange} />
                </p>
                <p>
                    <input type="date"
                        placeholder="Date Registration"
                        value={this.state.price}
                        onChange={this.onPriceChange} />
                </p>
                <p>
                    <input type="date"
                        placeholder="Date Last Activity"
                        value={this.state.price}
                        onChange={this.onPriceChange} />
                </p>

                <input type="submit" value="Save" />
            </form>
        );
    }
}


class InputTable extends React.Component {

    constructor(props) {
        super(props);
        this.state = { activitys: [] };

        this.onAdd = this.onAdd.bind(this);
        this.onRemove = this.onRemove.bind(this);
    }

    // Load data
    loadData() {
        var xhr = new XMLHttpRequest();
        xhr.open("GetUserActivitys", this.props.apiUrl, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ activitys: data });
        }.bind(this);
        xhr.send();
    }
    componentDidMount() {
        this.loadData();
    }

    // Add object
    onAdd(activity) {
        if (activity) {

            const data = new FormData();
            data.append("name", activity.name);
            data.append("price", activity.price);
            var xhr = new XMLHttpRequest();

            xhr.open("post", this.props.apiUrl, true);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send(data);
        }
    }
    // delete object
    onRemove(activity) {

        if (activity) {
            var url = this.props.apiUrl + "/" + activity.id;

            var xhr = new XMLHttpRequest();
            xhr.open("delete", url, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onload = function () {
                if (xhr.status === 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send();
        }
    }
    render() {

        var remove = this.onRemove;
        return <div>
            <Form onSubmit={this.onAdd} />
            <h2>Table activitys</h2>
            <div>
                {
                    this.state.activitys.map(function (activity) {

                        return <Activity key={activity.id} activity={activity} onRemove={remove} />
                    })
                }
            </div>
        </div>;
    }
}

ReactDOM.render(
    <InputTable apiUrl="/api/UserActivity" />,
    document.getElementById("app")
);
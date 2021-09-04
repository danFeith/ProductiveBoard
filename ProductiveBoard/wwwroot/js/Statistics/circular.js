// set the dimensions and margins of the graph
var width = 450
var height = 370

// append the svg object to the body of the page
var svg = d3.select("#circular_chart")
    .append("svg")
    .attr("width", width)
    .attr("height", height)

svg.append("circle").attr("cx", 130).attr("cy", 330).attr("r", 6).style("fill", "#F8766D")
svg.append("circle").attr("cx", 250).attr("cy", 330).attr("r", 6).style("fill", "#00BA38")
svg.append("text").attr("x", 150).attr("y", 330).text("User").style("font-size", "15px").attr("alignment-baseline", "middle")
svg.append("text").attr("x", 270).attr("y", 330).text("Admin").style("font-size", "15px").attr("alignment-baseline", "middle")

// create dummy data -> just one element per circle
$.get(`/users/usersroles`, (data) => {
    // create a tooltip
    var Tooltip = d3.select("#circular_chart")
        .append("div")
        .style("opacity", 0)
        .attr("class", "tooltip")
        .style("background-color", "white")
        .style("border", "solid")
        .style("border-width", "2px")
        .style("border-radius", "5px")
        .style("padding", "5px")

    // Three function that change the tooltip when user hover / move / leave a cell
    var mouseover = function (d) {
        Tooltip
            .style("opacity", 1)
        d3.select(this)
            .style("stroke", "#8004a1")
            .style("opacity", 1)
    }
    var mousemove = function (d) {
        Tooltip
            .html(d.name)
            .style("left", (d3.mouse(this)[0] + 70) + "px")
            .style("top", (d3.mouse(this)[1]) + "px")
    }
    var mouseleave = function (d) {
        Tooltip
            .style("opacity", 0)
        d3.select(this)
            .style("stroke", "black")
            .style("opacity", 0.8)
    }

    // A scale that gives a X target position for each group
    var x = d3.scaleOrdinal()
        .domain([1, 2])
        .range([50, 200])

    // A color scale
    var color = d3.scaleOrdinal()
        .domain([1, 2])
        .range(["#F8766D", "#00BA38"])

    // Initialize the circle: all located at the center of the svg area
    var node = svg.append("g")
        .selectAll("circle")
        .data(data)
        .enter()
        .append("circle")
        .attr("r", 29)
        .attr("cx", width / 2)
        .attr("cy", height / 2)
        .style("fill", function (d) { return color(d.role) })
        .style("fill-opacity", 0.8)
        .attr("stroke", "black")
        .style("stroke-width", 4)
        .call(d3.drag() // call specific function when circle is dragged
            .on("start", dragstarted)
            .on("drag", dragged)
            .on("end", dragended))
        .on("mouseover", mouseover)
        .on("mousemove", mousemove)
        .on("mouseleave", mouseleave);

    // Features of the forces applied to the nodes:
    var simulation = d3.forceSimulation()
        .force("x", d3.forceX().strength(0.5).x(function (d) { return x(d.role) }))
        .force("y", d3.forceY().strength(0.1).y(height / 2))
        .force("center", d3.forceCenter().x(width / 2).y(height / 2)) // Attraction to the center of the svg area
        .force("charge", d3.forceManyBody().strength(1)) // Nodes are attracted one each other of value is > 0
        .force("collide", d3.forceCollide().strength(.1).radius(32).iterations(1)) // Force that avoids circle overlapping

    // Apply these forces to the nodes and update their positions.
    // Once the force algorithm is happy with positions ('alpha' value is low enough), simulations will stop.
    simulation
        .nodes(data)
        .on("tick", function (d) {
            node
                .attr("cx", function (d) { return d.x; })
                .attr("cy", function (d) { return d.y; })
        });

    // What happens when a circle is dragged?
    function dragstarted(d) {
        if (!d3.event.active) simulation.alphaTarget(.03).restart();
        d.fx = d.x;
        d.fy = d.y;
    }
    function dragged(d) {
        d.fx = d3.event.x;
        d.fy = d3.event.y;
    }
    function dragended(d) {
        if (!d3.event.active) simulation.alphaTarget(.03);
        d.fx = null;
        d.fy = null;
    }
});
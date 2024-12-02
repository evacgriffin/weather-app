# Weather App

## Table of Contents

- [Description](#description)
- [Run the App](#run-the-app)
- [Usage](#usage)
- [Screenshots](#screenshots)
- [License](#license)
- [Roadmap](#roadmap)

## Description
The **Weather App** allows users to manually add, edit, and delete data to manage a weather database. The app connects to four microservices to obtain data measured by a sensor, display an image and notification related to the current temperature, 
and generate a graph of temperature data.

## Run the App

Pending...

## Usage

Users can click "Add New Data" to manually add new weather data to the database.

Users can click "Edit" next to a data point to edit the data point.

Users can click "Delete" next to a data point to delete the data point.

Users can filter the data by location.

**Note: To use the following features, the Weather Sensor, Weather Image, and Weather Notification microservices must be running.**
Users can click the "Request Sensor Data" button to request new data from the weather sensor microservice. The data will be saved to the database and displayed in the table after 1 second.
Once a new data point is received, the weather image microservice is called. An image matching the current temperature is displayed in the Weather App.
Additionally, the Weather Notification microservice is called and a matching notification will be displayed in the Weather App.

**Note: To generate a graph, the Graph Generator microservice must be running.**
Users can click the "Generate Graph" button. All temperature data currently stored in the database will be sent to the Graph Generator microservice. The microservice will generate a graph, plotting
the temperature for each date. The graph will be saved on the microservice side, but a confirmation will be displayed in the Weather App.

## Screenshots
![main-page](/screenshots/main_page.png)  
Homepage of the Weather App  


![add-data](/screenshots/manually_add.png)  
Manually adding data  


![edit-data](/screenshots/edit_data.png)  
Editing data  


![delete-data](/screenshots/delete_data.png)  
Deleting data  


![notification-and-image](/screenshots/notification_and_image.png)  
Image and notification displayed after receiving new sensor data  


![graph-confirmation](/screenshots/graph_confirmation.png)  
Confirmation displayed after generating a graph  



## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Roadmap

- [ ] Automatically request new sensor data based on a timer
- [ ] Display graph in the main app
- [ ] Request graph of humidity
- [ ] Add sort
- [ ] Implement "next" and "previous" buttons to view more data
- [ ] Expand to handle multiple sensors

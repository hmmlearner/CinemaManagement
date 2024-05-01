# EcommerceReact 

This is Movie booking application with APIs built on .NET 7 and SQLExpress for DB. Front end hasn't been created yet.

Users are able to book seats to the Movie based on show timings. Once the user chooses a seat for the preferred show, the seat will be held for 15 mins for the chosen showtime. If the user doesn't confirm the booking within 15 mins will receive 400 Bad request with "Booking expired" message. DeleteOldBookings API has been created to remove expired bookings. 

Unit tests are created in xUnit. 
  

## Fork the repository 


## Setup DB 

In the Package Manager Console run the below commands 

1. Add-Migration InitialCreate -Context CinemaContext 

2. Update-Database -Context CinemaContext 

  

## To start the project in Visual Studio 

1. Build solution and run 
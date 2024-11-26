# Use an official Python runtime as a parent image
FROM python:3.11-slim

# Set the working directory in the container
WORKDIR /backend

# Copy the current directory contents into the container at /usr/src/app
COPY . .

# Install any needed packages specified in requirements.txt 
RUN pip install --no-cache-dir -r requirements.txt

# Run additional setup script before starting the app
RUN python DevDbSetup.py

# Make port 80 available to the world outside this container (Optional, only for web apps)
EXPOSE 80
EXPOSE 5000

# Run app.py when the container launches
LABEL description="MantaDive - Backend"

ENTRYPOINT [ "python" ]
CMD ["Backend.py"]
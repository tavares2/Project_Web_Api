# Use the official Node.js image
FROM node:18

# Set the working directory
WORKDIR /app

# Install Mockoon CLI globally
RUN npm install -g @mockoon/cli

# Copy the Mockoon environment JSON file into the container
COPY LGC_CodeChallenge_TestApi.json /app/LGC_CodeChallenge_TestApi.json
# Expose port 3000 for the Mockoon API
EXPOSE 3000

# Command to run Mockoon CLI with the provided environment file
CMD ["mockoon-cli", "start", "--data", "/app/LGC_CodeChallenge_TestApi.json", "--port", "3000"]

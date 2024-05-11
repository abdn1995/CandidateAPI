import requests
import random
from faker import Faker

# Initialize Faker to generate random data
fake = Faker()

# API endpoint
API_ENDPOINT = 'http://localhost:3001/api/candidates'

# Function to generate a random candidate
def generate_random_candidate():
    # Adjust field names according to your Candidate model in the API
    return {
        "FirstName": fake.first_name(),
        "LastName": fake.last_name(),
        "Email": fake.email(),
        "PhoneNumber": fake.phone_number(),
        "TimeInterval": f"{random.randint(8, 12)}-{random.randint(13, 16)}",  # Adjusted name if necessary
        "LinkedInProfileUrl": f"https://www.linkedin.com/in/{fake.user_name()}",  # Adjusted name if necessary
        "GitHubProfileUrl": f"https://github.com/{fake.user_name()}",  # Adjusted name if necessary
        "Comment": fake.text(max_nb_chars=200)  # Adjusted name if necessary
    }

# Number of candidates to generate
NUM_CANDIDATES = 30

# Sending POST requests to the API endpoint
for _ in range(NUM_CANDIDATES):
    candidate = generate_random_candidate()
    print("Generated Candidate:", candidate)
    try:
        response = requests.post(API_ENDPOINT, json=candidate)
        print(f"Status Code: {response.status_code}, Response: {response.json()}")
    except Exception as e:
        print(f"An error occurred: {e}")

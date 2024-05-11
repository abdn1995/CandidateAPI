import requests
import random
from faker import Faker

# Initialize Faker to generate random data
fake = Faker()

# API endpoint
API_ENDPOINT = 'http://localhost:3001/api/candidates'

# Function to generate a random candidate
def generate_random_candidate():
    return {
        "FirstName": fake.first_name(),
        "LastName": fake.last_name(),
        "Email": fake.email(),
        "PhoneNumber": fake.phone_number(),
        "BestTimeToCall": f"{random.randint(8, 12)}-{random.randint(13, 16)}",
        "LinkedInUrl": f"https://www.linkedin.com/in/{fake.user_name()}",
        "GitHubUrl": f"https://github.com/{fake.user_name()}",
        "Comments": fake.text(max_nb_chars=200)
    }

# Number of candidates to generate
NUM_CANDIDATES = 30

# Sending POST requests to the API endpoint
for _ in range(NUM_CANDIDATES):
    candidate = generate_random_candidate()
    response = requests.post(API_ENDPOINT, json=candidate)
    print(f"Status Code: {response.status_code}, Response: {response.json()}")

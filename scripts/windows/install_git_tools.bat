@echo off
pip install pre-commit

git config core.commentchar ';'
pre-commit install -t pre-commit
pre-commit install -t commit-msg
